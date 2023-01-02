namespace MHFZASS.structs
{
    struct ArmorSet
    {
        public Armor head;
        public Armor torso;
        public Armor arms;
        public Armor waist;
        public Armor legs;

        public Resistances resistances;
        public List<string> activatedSkills;

        public ArmorSet()
        {
            head = new Armor();
            torso = new Armor();
            arms = new Armor();
            waist = new Armor();
            legs = new Armor();
            resistances = new Resistances();
            activatedSkills = new List<string>();
        }

        // Check the stat based conditions.
        public Resistances CalculateResistances()
        {
            Resistances res = new Resistances();
            Armor[] pieces = new Armor[] { head, torso, arms, waist, legs };

            foreach (Armor armor in pieces)
            {
                double defenseMultiplier = 0.05 * 7;
                int defenseModifier = (int)Math.Ceiling(armor.baseDefense * defenseMultiplier);
                res.defense += armor.baseDefense + defenseModifier;

                switch (HasSkill("防御"))
                {
                    case 10:
                        res.defense += 20;
                        break;
                    case 15:
                        res.defense += 30;
                        break;
                    case 25:
                        res.defense += 60;
                        break;
                    case 35:
                        res.defense += 90;
                        break;
                    case 45:
                        res.defense += 120;
                        break;
                }

                res.fire += armor.fireRes + HasSkill("火耐性");
                res.water += armor.waterRes + HasSkill("水耐性");
                res.ice += armor.iceRes + HasSkill("氷耐性");
                res.dragon += armor.dragonRes + HasSkill("龍耐性");
                res.thunder += armor.thunderRes + HasSkill("雷耐性");
            }

            return res;
        }

        public int GetSlots()
        {
            int totalSlots = 0;

            totalSlots += head.maxSlots;
            totalSlots += torso.maxSlots;
            totalSlots += arms.maxSlots;
            totalSlots += waist.maxSlots;
            totalSlots += legs.maxSlots;

            return totalSlots;
        }

        // Check if the set has a skill.
        public int HasSkill(string name)
        {
            Dictionary<string, int> map = GetSkillMap();
            return map.ContainsKey(name) == true ? map[name] : 0;
        }

        // Get all of the skills on a piece of gear.
        public Dictionary<string, int> GetSkillMap ()
        {
            Dictionary<string, int> skills = new Dictionary<string, int>();
            foreach (Skill skill in head.GetSkills())
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in torso.GetSkills())
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in arms.GetSkills())
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in waist.GetSkills())
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }
            foreach (Skill skill in legs.GetSkills())
            {
                if (!skills.ContainsKey(skill.name)) skills[skill.name] = 0;
                skills[skill.name] += skill.points;
            }

            return skills;
        }
    }
}
