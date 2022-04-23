using UnityEngine;
using System.Collections;
using MiniJameGam9.Character;

namespace MiniJameGam9.Weapon
{
    public class Grenade : MonoBehaviour
    {
        public int Damage { set; get; }
        public float TimeBeforeExplode { set; get; }
        public float ExplosionRadius { set; get; }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag("Player"))
                Explode();
            else
                StartCoroutine(WaitForExplode());
        }

        private IEnumerator WaitForExplode()
        {
            yield return new WaitForSeconds(TimeBeforeExplode);
            Explode();
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

            foreach (Collider collider in colliders)
            {
                if (collider.tag == "Player")
                {
                    Debug.Log("Hit player");
                    collider.GetComponent<ACharacterController>().TakeDamage(Damage);
                }
            }
            Destroy(gameObject);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
        }
    }
}
