using MHFZASS.structs;

namespace MHFZASS
{
    public partial class Form1 : Form
    {
        static List<Skill> skills = new List<Skill>();
        static List<Armor> armors = new List<Armor>();
        static CancellationTokenSource cancelToken = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadArmors();
            LoadArmorSkills();

            foreach (Skill skill in skills)
            {
                comboBox1.Items.Add(skill.name);
                comboBox2.Items.Add(skill.name);
                comboBox3.Items.Add(skill.name);
                comboBox4.Items.Add(skill.name);
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
        }

        bool ShouldTerminate(ArmorSet set, int count)
        {
            // Truncate here
            if (count > 1000) return true;
            //if (DateTime.Now.Subtract(startTime).Seconds > 5) return true; // It took too long, cancel.
            return false;
        }

        List<ArmorSet> FindSets()
        {
            // Used to automatically terminate a search after a while.
            DateTime startTime = DateTime.Now;
            List<Armor> matchesConditions = FindMatchingSets();

            // A list of combinations fitting conditions.
            //List<ArmorSet> matchingCombinations = new List<ArmorSet>();
            Task.Run(() => StartCalculatingSets(matchesConditions));

            return new List<ArmorSet>();
        }

        async void StartCalculatingSets(List<Armor> matchesConditions)
        {
            // Get all armors for certain body parts based on the previous matches.
            // This will filter down from previous results.
            List<Armor> matchedHelmets = GetArmorsBySlot(matchesConditions, "頭");
            List<Armor> matchedTorsos = GetArmorsBySlot(matchesConditions, "胴");
            List<Armor> matchedArms = GetArmorsBySlot(matchesConditions, "腕");
            List<Armor> matchedWaists = GetArmorsBySlot(matchesConditions, "腰");
            List<Armor> matchedLegs = GetArmorsBySlot(matchesConditions, "脚");

            foreach (Armor helmet in matchedHelmets)
            {
                ArmorSet set = new ArmorSet();
                if (ShouldTerminate(set, treeView1.Nodes.Count)) return;
                if (cancelToken.IsCancellationRequested) return;
                set.head = helmet;

                foreach (Armor torso in matchedTorsos)
                {   
                    if (ShouldTerminate(set, treeView1.Nodes.Count)) return;
                    if (cancelToken.IsCancellationRequested) return;
                    set.torso = torso;

                    foreach (Armor arms in matchedArms)
                    {
                        if (ShouldTerminate(set, treeView1.Nodes.Count)) return;
                        if (cancelToken.IsCancellationRequested) return;
                        set.arms = arms;

                        foreach (Armor waist in matchedWaists)
                        {
                            if (ShouldTerminate(set, treeView1.Nodes.Count)) return;
                            if (cancelToken.IsCancellationRequested) return;
                            set.waist = waist;

                            foreach (Armor legs in matchedLegs)
                            {
                                if (ShouldTerminate(set, treeView1.Nodes.Count)) return;
                                if (cancelToken.IsCancellationRequested) return;

                                set.legs = legs;
                                set.resistances = set.CalculateResistances();

                                AddToList(set);
                                UpdateProgressBar(1000);
                            }
                        }
                    }
                }
            }
        }

        void UpdateProgressBar(int total)
        {
            if (InvokeRequired)
            {
                Invoke((Action<int>)UpdateProgressBar, total);
                return;
            }

            progressBar1.Value = (treeView1.Nodes.Count / total) * 100;
            label11.Text = treeView1.Nodes.Count.ToString();
            
        }

        List<Armor> GetArmorsBySlot(List<Armor> matching, string type)
        {
            List<Armor> list = new List<Armor>();
            
            foreach(Armor armor in matching)
            {
                if (armor.equipClass == type) list.Add(armor);
            }

            return list;
        }

        void AddToList(ArmorSet set)
        {
            if (InvokeRequired)
            {
                Invoke((Action<ArmorSet>)AddToList, set);
                return;
            }

            List<string> skillList = FitsConditions(set);

            if (skillList == null) return;

            set.activatedSkills = skillList;
            AddSetToTree(set);
        }

        List<string> FitsConditions(ArmorSet set)
        {
            if (!FitsSearchConditions(set)) return null;

            Dictionary<string, int> skills = set.GetSkillMap();
            List<string> activatedSkills = new List<string>();

            foreach(string skill in skills.Keys)
            {
                if (skills[skill] >= 10) activatedSkills.Add(skill);
            }

            // Take all of the desired skills and compare them to what we have.
            string[] needed = new string[] { comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), comboBox4.SelectedItem.ToString() };
            // Check if the activated skills are present
            foreach(string skill in needed)
            {
                // If you have that skill, move on.
                if (activatedSkills.ToArray().Contains(skill)) continue;
                if (skill != "なし") return null; // Else skip this set.
            }

            // Check if the skill's level is high enough
            if (comboBox1.SelectedItem.ToString() != "なし" && skills[comboBox1.SelectedItem.ToString()] < numericUpDown7.Value) return null;
            if (comboBox2.SelectedItem.ToString() != "なし" && skills[comboBox2.SelectedItem.ToString()] < numericUpDown8.Value) return null;
            if (comboBox3.SelectedItem.ToString() != "なし" && skills[comboBox3.SelectedItem.ToString()] < numericUpDown9.Value) return null;
            if (comboBox4.SelectedItem.ToString() != "なし" && skills[comboBox4.SelectedItem.ToString()] < numericUpDown10.Value) return null;
            return activatedSkills;
        }

        bool FitsSearchConditions(ArmorSet set)
        {
            // This is an easier way to access the minimum values for everything.
            int minimumDefense = (int)numericUpDown1.Value,
                minimumFire = (int)numericUpDown10.Value,
                minimumWater = (int)numericUpDown9.Value,
                minimumThunder = (int)numericUpDown8.Value,
                minimumIce = (int)numericUpDown7.Value,
                minimumDragon = (int)numericUpDown6.Value;

            // Check if any of the resistances does not fit within the search conditions
            if (set.resistances.defense < minimumDefense
                || set.resistances.fire < minimumFire
                || set.resistances.water < minimumWater
                || set.resistances.thunder < minimumThunder
                || set.resistances.ice < minimumIce
                || set.resistances.dragon < minimumDragon) return false;

            return true;
        }

        // Find a smaller list of armors that have the skill you want.
        List<Armor> FindMatchingSets()
        {
            List<Armor> matchingArmors = new List<Armor>();

            int minimumDefense = (int)numericUpDown1.Value;
            string skill1 = comboBox1.SelectedItem.ToString();
            string skill2 = comboBox2.SelectedItem.ToString();
            string skill3 = comboBox3.SelectedItem.ToString();
            string skill4 = comboBox4.SelectedItem.ToString();

            int gender = comboBox6.SelectedIndex;
            int weaponType = comboBox5.SelectedIndex;

            // Zero out any null options
            if (skill1 == "なし") skill1 = "";
            if (skill2 == "なし") skill2 = "";
            if (skill3 == "なし") skill3 = "";
            if (skill4 == "なし") skill4 = "";

            // Determine armors matching the base conditions.
            foreach (Armor armor in armors)
            {
                if (!armor.isMaleEquip && gender == 0) continue;
                if (!armor.isFemaleEquip && gender == 1) continue;

                if (!armor.isBladeEquip && weaponType == 0) continue;
                if (!armor.isGunnerEquip && weaponType == 1) continue; 
                
                // Skip over bad results.
                if (armor.baseDefense < (minimumDefense / 8)) continue;

                string[] skills = new string[] { armor.skillId1, armor.skillId2, armor.skillId3, armor.skillId4 };

                if (skills.Contains(skill1) || skills.Contains(skill2) || skills.Contains(skill3) || skills.Contains(skill4))
                    matchingArmors.Add(armor);
            }

            return matchingArmors;
        }

        static void LoadArmors()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var reader = new StreamReader("./Armors.csv", System.Text.Encoding.GetEncoding(932)))
            using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<Armor>();

                foreach (Armor armor in records)
                {
                    armors.Add(armor);
                }
            }
        }


        static void LoadArmorSkills()
        {
            List<string> eSkills = new List<string>();

            foreach (Armor armor in armors)
            {
                if (!eSkills.Contains(armor.skillId1))
                    eSkills.Add(armor.skillId1);
            }

            foreach(string eskill in eSkills)
            {
                Skill skill = new Skill();
                skill.name = eskill;
                skills.Add(skill);
            }
        }

        static List<ArmorSet> SortSets(List<ArmorSet> sets)
        {
            sets.Sort(SortByDefense);
            return sets;
        }

        static int SortByDefense(ArmorSet set, ArmorSet set2)
        {
            if (set.resistances.defense < set2.resistances.defense) return 1;
            if (set.resistances.defense > set2.resistances.defense) return -1;
            return 0;
        }

        void AddSetToTree(ArmorSet armorSet)
        {
            TreeNode node = treeView1.Nodes.Add("Set " + treeView1.Nodes.Count);

            node.Nodes.Add(armorSet.head.name);
            node.Nodes.Add(armorSet.torso.name);
            node.Nodes.Add(armorSet.arms.name);
            node.Nodes.Add(armorSet.waist.name);
            node.Nodes.Add(armorSet.legs.name);

            // Defenses and elemental resistances
            node.Nodes.Add("Defense: " + armorSet.resistances.defense);

            node.Nodes.Add("Fire: " + armorSet.resistances.fire);
            node.Nodes.Add("Water: " + armorSet.resistances.water);
            node.Nodes.Add("Thunder: " + armorSet.resistances.thunder);
            node.Nodes.Add("Dragon: " + armorSet.resistances.dragon);
            node.Nodes.Add("Ice: " + armorSet.resistances.ice);

            node.Nodes.Add($"Skills ({armorSet.activatedSkills.Count}):");

            foreach (string skill in armorSet.activatedSkills)
            {
                node.Nodes.Add(skill);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Stop")
            {
                button1.Text = "Search";
                cancelToken.Cancel();
            }

            treeView1.Nodes.Clear(); // Clear the tree
            button1.Text = "Stop";

            List<ArmorSet> compatibleSets = FindSets();
            compatibleSets = SortSets(compatibleSets);

            button1.Text = "Search";
        }
    }
}