using MiniJameGam9.Character;
using UnityEngine;

namespace MiniJameGam9.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public int Damage { set; get; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                collision.collider.GetComponent<ACharacterController>().TakeDamage(Damage);
            }
            Destroy(gameObject);
        }
    }
}
