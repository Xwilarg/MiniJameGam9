using UnityEngine;

namespace MiniJameGam9.Character
{
    public class Profile
    {
        public Profile(bool isAi, string name, Camera camera = null)
            => (IsAi, Name, Kill, Death, DamageDealt, Camera)
            = (isAi, name, 0, 0, 0, camera);

        public bool IsAi { get; set; }
        public string Name { get; set; }
        public int Kill { get; set; }
        public int Death { get; set; }
        public int DamageDealt { get; set; }
        public Camera Camera { get; set; }
    }
}
