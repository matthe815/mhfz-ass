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
                res.fire += armor.fireRes;
                res.water += armor.waterRes;
                res.ice += armor.iceRes;
                res.dragon += armor.dragonRes;
                res.thunder += armor.thunderRes;
            }

            return res;
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
