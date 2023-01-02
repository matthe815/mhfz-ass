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

        class SkillListItem
        {
            public string id;
            string name;

            public SkillListItem(string id, string name)
            {
                this.id = id;
                this.name = name;
            }

            public override string ToString()
            {
                return this.name;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadArmors();

            int lineNum = 0;

            foreach (Skill skill in skills)
            {
                comboBox1.Items.Add(new SkillListItem(skill.name, skill.en_name));
                comboBox2.Items.Add(new SkillListItem(skill.name, skill.en_name));
                comboBox3.Items.Add(new SkillListItem(skill.name, skill.en_name));
                comboBox4.Items.Add(new SkillListItem(skill.name, skill.en_name));
                lineNum++;
            }

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            comboBox4.SelectedIndex = 0;
            comboBox5.SelectedIndex = 0;
            comboBox6.SelectedIndex = 0;
            comboBox7.SelectedIndex = 0;
        }

        bool ShouldTerminate(ArmorSet set, int count)
        {
            // Truncate here
            if (count >= 1000) return true;
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
            var progress = new Progress<int>(percent =>
            {
                if (percent > 100) percent = 100;
                progressBar1.Value = percent;
            });

            Task.Run(() => StartCalculatingSets(matchesConditions, progress));

            return new List<ArmorSet>();
        }

        void StartCalculatingSets(List<Armor> matchesConditions, IProgress<int> progress)
        {
            // Get all armors for certain body parts based on the previous matches.
            // This will filter down from previous results.
            List<Armor> matchedHelmets = GetArmorsBySlot(matchesConditions, "頭");
            List<Armor> matchedTorsos = GetArmorsBySlot(matchesConditions, "胴");
            List<Armor> matchedArms = GetArmorsBySlot(matchesConditions, "腕");
            List<Armor> matchedWaists = GetArmorsBySlot(matchesConditions, "腰");
            List<Armor> matchedLegs = GetArmorsBySlot(matchesConditions, "脚");

            int matched = 0;

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

                                matched++;
                                progress.Report((matched / 1000) * 100);
                            }
                        }
                    }
                }
            }

            progress.Report(100);
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

            SkillListItem skill1 = comboBox1.SelectedItem as SkillListItem;
            SkillListItem skill2 = comboBox2.SelectedItem as SkillListItem;
            SkillListItem skill3 = comboBox3.SelectedItem as SkillListItem;
            SkillListItem skill4 = comboBox4.SelectedItem as SkillListItem;

            // Take all of the desired skills and compare them to what we have.
            string[] needed = new string[] { skill1.id, skill2.id, skill3.id, skill4.id };
            // Check if the activated skills are present
            foreach(string skill in needed)
            {
                // If you have that skill, move on.
                if (activatedSkills.ToArray().Contains(skill)) continue;
                if (skill != "なし") return null; // Else skip this set.
            }



            // Check if the skill's level is high enough
            if (skill1.id != "なし" && skills[skill1.id] < numericUpDown7.Value) return null;
            if (skill2.id != "なし" && skills[skill2.id] < numericUpDown8.Value) return null;
            if (skill3.id != "なし" && skills[skill3.id] < numericUpDown9.Value) return null;
            if (skill4.id != "なし" && skills[skill4.id] < numericUpDown10.Value) return null;
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
            string skill1 = ((SkillListItem)comboBox1.SelectedItem).id;
            string skill2 = ((SkillListItem)comboBox2.SelectedItem).id;
            string skill3 = ((SkillListItem)comboBox3.SelectedItem).id;
            string skill4 = ((SkillListItem)comboBox4.SelectedItem).id;

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

                if (comboBox7.SelectedIndex < 1 && (armor.name.Contains("G") || armor.name.Contains("Ｇ"))) continue;
                if (comboBox7.SelectedIndex < 2 && (armor.name.Contains("Z") || armor.name.Contains("Ｚ"))) continue;

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

            LoadArmorSkills(); // Load skills before sorting armor.

            armors.Sort((armor1, armor2) =>
            {
                if (armor1.baseDefense < armor2.baseDefense) return 1;
                if (armor1.baseDefense > armor2.baseDefense) return -1;

                return 0;
            });
        }


        static void LoadArmorSkills()
        {
            List<string> eSkills = new List<string>();
            string[] translations = File.ReadAllLines("./skills_en.txt");

            int lineNum = 0;

            foreach (Armor armor in armors)
            {
                if (!eSkills.Contains(armor.skillId1))
                    eSkills.Add(armor.skillId1);
            }

            foreach(string eskill in eSkills)
            {
                Skill skill = new Skill();
                skill.name = eskill;
                skill.en_name = translations[lineNum];
                skills.Add(skill);
                lineNum++;
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

            node.Nodes.Add(armorSet.head.GetName());
            node.Nodes.Add(armorSet.torso.GetName());
            node.Nodes.Add(armorSet.arms.GetName());
            node.Nodes.Add(armorSet.waist.GetName());
            node.Nodes.Add(armorSet.legs.GetName());

            node.Nodes.Add($"Slots: {armorSet.GetSlots()}");

            node.Nodes.Add("------");

            // Defenses and elemental resistances
            node.Nodes.Add("Defense: " + armorSet.resistances.defense);

            node.Nodes.Add("--- Damage Reduction --- ");
            double damageReduction = Math.Ceiling((armorSet.resistances.defense * 0.2));
            node.Nodes.Add($"Physical: {damageReduction}%");
            node.Nodes.Add($"Fire: {armorSet.resistances.fire}%");
            node.Nodes.Add($"Water: {armorSet.resistances.water}%");
            node.Nodes.Add($"Thunder: {armorSet.resistances.thunder}%");
            node.Nodes.Add($"Ice: {armorSet.resistances.ice}%");
            node.Nodes.Add($"Dragon: {armorSet.resistances.dragon}%");

            node.Nodes.Add($"--- Skills ({armorSet.activatedSkills.Count}) ---");

            node.Nodes.Add($"Skills ({armorSet.activatedSkills.Count}):");

            foreach (KeyValuePair<string, int> skill in armorSet.GetSkillMap())
            {
                node.Nodes.Add($"{LocalizeKey(skill.Key)}: {skill.Value}");
            }
        }

        static string LocalizeKey (string key)
        {
            Skill skill = skills.Find((skill) => skill.name == key);
            return skill.en_name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear(); // Clear the tree

            List<ArmorSet> compatibleSets = FindSets();
            compatibleSets = SortSets(compatibleSets);
        }
    }
}