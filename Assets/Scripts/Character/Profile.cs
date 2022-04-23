namespace MiniJameGam9.Character
{
    public class Profile
    {
        public Profile(bool isAi, string name)
            => (IsAi, Name, Kill, Death, DamageDealt)
            = (isAi, name, 0, 0, 0);

        public bool IsAi { get; set; }
        public string Name { get; set; }
        public int Kill { get; set; }
        public int Death { get; set; }
        public int DamageDealt { get; set; }
    }
}
