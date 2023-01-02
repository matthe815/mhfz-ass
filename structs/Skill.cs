namespace MHFZASS.structs
{
    struct Skill
    {
        public string name;
        public string en_name;
        public int points;

        public Skill(string name, int points)
        {
            this.name = name;
            this.en_name = "";
            this.points = points;
        }
    }
}