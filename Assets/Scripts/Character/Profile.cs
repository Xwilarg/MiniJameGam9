using UnityEngine;

namespace MiniJameGam9.Character
{
    public class Profile
    {
        public Profile(bool isAi, string name, Camera camera = null, InputContainer container = null)
            => (IsAi, Name, Kill, Assist, Death, DamageDealt, Camera, Container)
            = (isAi, name, 0, 0, 0, 0, camera, container);

        public bool IsAi { get; set; }
        public string Name { get; set; }
        public int Assist { get; set; }
        public int Kill { get; set; }
        public int Death { get; set; }
        public int DamageDealt { get; set; }
        public Camera Camera { get; set; }
        public InputContainer Container { get; set; }

        public static bool operator ==(Profile a, Profile b)
        {
            if (a is null)
            {
                return b is null;
            }
            if (b is null)
                return false;
            return a.IsAi == b.IsAi && a.Name == b.Name;
        }
        public static bool operator !=(Profile a, Profile b)
            => !(a == b);

        public override int GetHashCode()
            => (IsAi, Name).GetHashCode();
    }
}
