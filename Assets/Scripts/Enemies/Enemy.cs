using MiniJameGam9.Projectiles;
using UnityEngine;

namespace MiniJameGam9.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private EnemyStatsSO _baseStats;
        private EnemyStats _stats;
        [SerializeField] private Rigidbody _rigidbody;

        private void Start()
        {
            _stats = new EnemyStats(_baseStats);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag($"Bullet"))
            {
                TakeDamage(other.gameObject.GetComponent<BulletBehaviour>().GetDamage());
            }
        }

        private void TakeDamage(int damage)
        {
            _stats.HP -= damage;
            if(_stats.HP <= 0 )
                Destroy(gameObject);
        }
    }
}