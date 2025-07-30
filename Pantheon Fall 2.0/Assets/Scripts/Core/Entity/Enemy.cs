namespace Core.Entity
{
    public class Enemy : Entity
    {
        public string ShortName;
        public string FullName;

        public Enemy(string shortName, string fullName, EntityStats stats) : base(stats)
        {
            ShortName = shortName;
            FullName = fullName;
        }
    }
}