using MiniJameGam9.SO;
using UnityEngine;

namespace MiniJameGam9.Weapon
{
    public class WeaponCase : MonoBehaviour
    {
        [SerializeField]
        private WeaponInfo _weaponInfo;

        public DropCase Parent { set; get; }

        public WeaponInfo Take()
        {
            Parent.WaitAndRespawn();
            return _weaponInfo;
        }
    }
}
