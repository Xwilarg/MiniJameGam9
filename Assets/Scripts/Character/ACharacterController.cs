using MiniJameGam9.SO;
using UnityEngine;

namespace MiniJameGam9.Character
{
    public abstract class ACharacterController : MonoBehaviour
    {
        [SerializeField]
        private SO.CharacterInfo _cInfo;

        [SerializeField]
        private WeaponInfo _baseWeapon;

        [SerializeField]
        private Transform _gunOut;

        private int _health;

        private void Start()
        {
            _health = _cInfo.BaseHealth;
        }

        public void Shoot()
        {

        }

        public void TakeDamage(int value)
        {
            _health -= value;
            if (_health < 0)
            {
                _health = 0;
                Destroy(gameObject);
            }
        }
    }
}
