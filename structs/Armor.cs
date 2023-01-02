namespace MHFZASS.structs
{
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

        public List<Skill> GetSkills()
        {
            List<Skill> combinedSkills = new List<Skill>();
            Dictionary<string, int> skillPointMapping = new Dictionary<string, int>();
            List<Skill> finalSkills = new List<Skill>();

            combinedSkills.Add(new Skill(skillId1, skillPts1));
            combinedSkills.Add(new Skill(skillId2, skillPts2));
            combinedSkills.Add(new Skill(skillId3, skillPts3));
            combinedSkills.Add(new Skill(skillId4, skillPts4));

            foreach (Skill skill in combinedSkills)
            {
                if (!skillPointMapping.ContainsKey(skill.name)) skillPointMapping[skill.name] = 0;
                skillPointMapping[skill.name] += skill.points;
            }

            foreach (string key in skillPointMapping.Keys)
            {
                finalSkills.Add(new Skill(key, skillPointMapping[key]));
            }

            return finalSkills;
        }

        public string GetName()
        {
            string slottedName = $"{this.name} -- ";

            for (int i = 0; i < this.maxSlots; i++) slottedName += "O";

            return slottedName;
        }
    }
}
