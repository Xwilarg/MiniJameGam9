using MiniJameGam9.Character;
using UnityEngine;

namespace MiniJameGam9.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public int Damage { set; get; }
        public ACharacterController Author { set; get; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                Author.Profile.DamageDealt += Damage;
                if (collision.collider.GetComponent<ACharacterController>().TakeDamage(Damage))
                {
                    Author.Profile.Kill++;
                }
            }
            Destroy(gameObject);
        }
    }
}
