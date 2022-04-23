using UnityEngine;
using System.Collections;
using MiniJameGam9.Character;
using MiniJameGam9.SO;

namespace MiniJameGam9.Weapon
{
    public class Projectile : MonoBehaviour
    {
        public WeaponInfo Weapon { set; get; }

        private void OnCollisionEnter(Collision collision)
        {
            if (Weapon.ExplosionRadius > 0f)
                StartCoroutine(Explode());
            else
                Damage(collision.collider.transform);
        }

        private IEnumerator Explode()
        {
            yield return new WaitForSeconds(Weapon.TimeBeforeExplode);

            Collider[] colliders = Physics.OverlapSphere(transform.position, Weapon.ExplosionRadius);
            foreach (Collider collider in colliders)
                Damage(collider.transform);
        }

        private void Damage(Transform target)
        {
            if (target.CompareTag("Player"))
                target.GetComponent<ACharacterController>().TakeDamage(Weapon.Damage);

            Destroy(gameObject);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Weapon.ExplosionRadius);
        }
    }
}
