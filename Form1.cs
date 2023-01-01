using System.Collections.Generic;

namespace MHFZASS
{
    public partial class Form1 : Form
    {
        struct Skill
        {
            public string name;
            public int points;

            public Skill(string name, int points)
            {
                this.name = name;
                this.points = points;
            }
        }

        struct Armor
        {
            public String equipClass { get; set; }
            public String name { get; set; }
            public Int16 modelIdMale { get; set; }
            public Int16 modelIdFemale { get; set; }
            public Boolean isMaleEquip { get; set; }
            public Boolean isFemaleEquip { get; set; }
            public Boolean isBladeEquip { get; set; }
            public Boolean isGunnerEquip { get; set; }
            public Boolean bool1 { get; set; }
            public Boolean isSPEquip { get; set; }
            public Boolean bool3 { get; set; }
            public Boolean bool4 { get; set; }
            public Byte rarity { get; set; }
            public Byte maxLevel { get; set; }
            public Byte unk1_1 { get; set; }
            public Byte unk1_2 { get; set; }
            public Byte unk1_3 { get; set; }
            public Byte unk1_4 { get; set; }
            public Byte unk2 { get; set; }
            public Int32 zennyCost { get; set; }
            public Int16 unk3 { get; set; }
            public Int16 baseDefense { get; set; }
            public SByte fireRes { get; set; }
            public SByte waterRes { get; set; }
            public SByte thunderRes { get; set; }
            public SByte dragonRes { get; set; }
            public SByte iceRes { get; set; }
            public Int16 unk3_1 { get; set; }
            public Byte baseSlots { get; set; }
            public Byte maxSlots { get; set; }
            public Byte sthEventCrown { get; set; }
            public Byte unk5 { get; set; }
            public Byte unk6 { get; set; }
            public Byte unk7_1 { get; set; }
            public Byte unk7_2 { get; set; }
            public Byte unk7_3 { get; set; }
            public Byte unk7_4 { get; set; }
            public Byte unk8_1 { get; set; }
            public Byte unk8_2 { get; set; }
            public Byte unk8_3 { get; set; }
            public Byte unk8_4 { get; set; }
            public Int16 unk10 { get; set; }
            public String skillId1 { get; set; }
            public SByte skillPts1 { get; set; }
            public String skillId2 { get; set; }
            public SByte skillPts2 { get; set; }
            public String skillId3 { get; set; }
            public SByte skillPts3 { get; set; }
            public String skillId4 { get; set; }
            public SByte skillPts4 { get; set; }
            public String skillId5 { get; set; }
            public SByte skillPts5 { get; set; }
            public Int32 sthHiden { get; set; }
            public Int32 unk12 { get; set; }
            public Byte unk13 { get; set; }
            public Byte unk14 { get; set; }
            public Byte unk15 { get; set; }
            public Byte unk16 { get; set; }
            public Int32 unk17 { get; set; }
            public Int16 unk18 { get; set; }
            public Int16 unk19 { get; set; }
        }

        struct ArmorSet
        {
            public Armor head;
            public Armor torso;
            public Armor arms;
            public Armor waist;
            public Armor legs;

            public Resistances resistances;

            public ArmorSet()
            {
                head = new Armor();
                torso = new Armor();
                arms = new Armor();
                waist = new Armor();
                legs = new Armor();
                resistances = new Resistances();
            }
        }

        static List<Skill> skills = new List<Skill>();
        static List<Armor> armors = new List<Armor>();

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
        }

        List<ArmorSet> FindSets()
        {
            List<ArmorSet> list = new List<ArmorSet>();

            List<Armor> matchesConditions = FindMatchingSets();
            List<Armor> helms = GetArmorOfType(matchesConditions, "頭");
            List<Armor> torsos = GetArmorOfType(matchesConditions, "胴");
            List<Armor> armsss = GetArmorOfType(matchesConditions, "腕");
            List<Armor> waists = GetArmorOfType(matchesConditions, "腰");
            List<Armor> legsss = GetArmorOfType(matchesConditions, "脚");

            int minimumDefense = (int)numericUpDown1.Value;
            int minimumFire = (int)numericUpDown10.Value;
            int minimumWater = (int)numericUpDown9.Value;
            int minimumThunder = (int)numericUpDown8.Value;
            int minimumIce = (int)numericUpDown7.Value;
            int minimumDragon = (int)numericUpDown6.Value;

            foreach (Armor helmet in helms)
            {

                ArmorSet set = new ArmorSet();
                set.head = helmet;

                // Truncate here
                if (list.Count > 1000) break;

                // Skip over bad results.
                if (helmet.baseDefense < (minimumDefense / 8)) continue;

                foreach (Armor torso in torsos)
                {
                    set.torso = torso;

                    // Truncate here
                    if (list.Count > 1000) break;

                    // Skip over bad results.
                    if (torso.baseDefense < (minimumDefense / 8)) continue;

                    foreach (Armor arms in armsss)
                    {
                        set.arms = arms;

                        // Truncate here
                        if (list.Count > 1000) break;

                        // Skip over bad results.
                        if (arms.baseDefense < (minimumDefense / 8)) continue;

                        foreach (Armor waist in waists)
                        {
                            set.waist = waist;

                            // Truncate here
                            if (list.Count > 1000) break;

                            // Skip over bad results.
                            if (waist.baseDefense < (minimumDefense / 8)) continue;

                            foreach (Armor legs in legsss)
                            {
                                // Skip over bad results.
                                if (legs.baseDefense < (minimumDefense / 8)) continue;

                                // Truncate here
                                if (list.Count > 1000) break;

                                set.legs = legs;

                                set.resistances = GetDefense(set);

                                if (set.resistances.defense < minimumDefense) continue;
                                if (set.resistances.fire < minimumFire) continue;
                                if (set.resistances.ice < minimumIce) continue;
                                if (set.resistances.thunder < minimumThunder) continue;
                                if (set.resistances.water < minimumWater) continue;
                                if (set.resistances.dragon < minimumDragon) continue;
                                if (!FitsConditions(set)) continue;

                                list.Add(set);
                            }
                        }
                    }
                }
            }

            return list;
        }

        List<Armor> GetArmorOfType(List<Armor> matching, string type)
        {
            List<Armor> list = new List<Armor>();
            
            foreach(Armor armor in matching)
            {
                if (armor.equipClass == type) list.Add(armor);
            }

            return list;
        }

        struct Resistances
        {
            public int defense;
            public int fire;
            public int water;
            public int thunder;
            public int ice;
            public int dragon;
        }

        // Check the stat based conditions.
        static Resistances GetDefense(ArmorSet set)
        {
            Resistances res = new Resistances();

            Armor[] pieces = new Armor[] { set.head, set.torso, set.arms, set.waist, set.legs };

            foreach (Armor armor in pieces)
            {
                double defenseMultiplier = 0.05 * 7;
                int defenseModifier = (int)Math.Ceiling(armor.baseDefense * defenseMultiplier);
                res.defense += armor.baseDefense + defenseModifier;
                res.fire += armor.fireRes;
                res.water += armor.waterRes;
                res.ice += armor.iceRes;
                res.dragon += armor.dragonRes;
                res.thunder += armor.thunderRes;
            }

            return res;
        }

        bool FitsConditions(ArmorSet set)
        {
            Dictionary<string, int> skills = new Dictionary<string, int>();
            foreach (Skill skill in GetCombinedSkills(set.head))
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in GetCombinedSkills(set.torso))
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in GetCombinedSkills(set.arms))
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in GetCombinedSkills(set.waist))
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in GetCombinedSkills(set.legs))
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            List<string> activatedSkills = new List<String>();
            foreach(string skill in skills.Keys)
            {
                if (skills[skill] >= 10) activatedSkills.Add(skill);
            }
            string[] needed = new string[] { comboBox1.SelectedItem.ToString(), comboBox2.SelectedItem.ToString(), comboBox3.SelectedItem.ToString(), comboBox4.SelectedItem.ToString() };
            foreach(string skill in needed)
            {
                // If you have that skill, move on.
                if (activatedSkills.ToArray().Contains(skill)) break;
                if (skill != "なし") return false; // Else skip this set.
            }
            return true;
        }

        List<Skill> GetCombinedSkills(Armor armor)
        {
            List<Skill> combinedSkills = new List<Skill>();
            Dictionary<string, int> skils = new Dictionary<string, int>();
            List<Skill> finalSkills = new List<Skill>();
            
            combinedSkills.Add(new Skill(armor.skillId1, armor.skillPts1));
            combinedSkills.Add(new Skill(armor.skillId2, armor.skillPts2));
            combinedSkills.Add(new Skill(armor.skillId3, armor.skillPts3));
            combinedSkills.Add(new Skill(armor.skillId4, armor.skillPts4));

            foreach (Skill skill in combinedSkills)
            {
                if (!skils.ContainsKey(skill.name)) skils[skill.name] = 0;
                skils[skill.name] += skill.points;
            }

            foreach (string key in skils.Keys)
            {
                finalSkills.Add(new Skill(key, skils[key]));
            }

            return finalSkills;
        }

        // Find a smaller list of armors that have the skill you want.
        List<Armor> FindMatchingSets()
        {
            List<Armor> matchingArmors = new List<Armor>();

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

        private void button1_Click(object sender, EventArgs e)
        {
            List<ArmorSet> compatibleSets = FindSets();
            compatibleSets = SortSets(compatibleSets);

            treeView1.Nodes.Clear(); // Empty the list.

            int setNumber = 0;

            foreach (ArmorSet armorSet in compatibleSets)
            {
                TreeNode node = treeView1.Nodes.Add("Set " + setNumber);

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

                setNumber++;
            }
        }
    }
}