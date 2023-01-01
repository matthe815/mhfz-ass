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

            foreach(Armor armor in matchesConditions)
            {
                if (list.Count >= 50) break;

                Armor torso = new Armor();
                Armor arms = new Armor();
                Armor waist = new Armor();
                Armor legs = new Armor();

                if (armor.equipClass == "頭")
                {
                    foreach(Armor armor1 in matchesConditions)
                    {
                        switch (armor1.equipClass)
                        {
                            case "胴":
                                if (torso.name == null) torso = armor1;
                                break;

                            case "腕":
                                if (arms.name == null) arms = armor1;
                                break;

                            case "腰":
                                if (waist.name == null) waist = armor1;
                                break;

                            case "脚":
                                if (legs.name == null) legs = armor1;
                                break;
                        }
                    }
                    ArmorSet set = new ArmorSet();
                    set.head = armor;
                    set.torso = torso;
                    set.arms = arms;
                    set.waist = waist;
                    set.legs = legs;

                    if (!FitsConditions(set)) continue;

                    list.Add(set);
                } else
                {
                    break;
                }
            }

            return list;
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

            if (skill2 == "なし") skill2 = "";
            if (skill3 == "なし") skill3 = "";
            if (skill4 == "なし") skill4 = "";

            // Determine armors matching the base conditions.
            foreach (Armor armor in armors)
            {
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
                    Console.WriteLine(armor.name);
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

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("Clicked");
            List<ArmorSet> compatibleSets = FindSets();
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

                setNumber++;
            }
        }
    }
}