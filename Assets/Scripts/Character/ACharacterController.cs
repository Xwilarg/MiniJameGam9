using UnityEngine;

namespace MiniJameGam9.Character
{
    public abstract class ACharacterController : MonoBehaviour
    {
        [SerializeField]
        private SO.CharacterInfo _cInfo;

        private int _health;

        private void Start()
        {
            _health = _cInfo.BaseHealth;
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
