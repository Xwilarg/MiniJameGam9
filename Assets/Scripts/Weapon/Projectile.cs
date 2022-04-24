using UnityEngine;
using System.Collections;
using MiniJameGam9.Character;
using MiniJameGam9.SO;
using MiniJameGam9.Achievement;

namespace MiniJameGam9.Weapon
{
    public class Projectile : MonoBehaviour
    {
        public Profile Profile { set; get; }
        public WeaponInfo Weapon { set; get; }
        public Vector3 ShootOrigin { set; get; }

        private void OnCollisionEnter(Collision collision)
        {
            if (Weapon.ExplosionRadius > 0f && !collision.collider.CompareTag("Player"))
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
            {
                Profile.DamageDealt += Weapon.Damage;
                if (target.GetComponent<ACharacterController>().TakeDamage(Weapon.Damage, (ShootOrigin - transform.position).normalized, Profile, Weapon.FragIcon))
                {
                    Profile.Kill++;
                    if (!Profile.IsAi)
                    {
                        AchievementManager.Instance.UpdateKillAchievement(Weapon);
                    }
                }
            }
            if (Weapon.DestroyProjectileEffect != null)
            {
                var particleFX = Instantiate(Weapon.DestroyProjectileEffect, transform.position, Quaternion.identity);
                particleFX.GetComponent<ParticleSystem>().Play();
                Destroy(particleFX, 1f);
            }
            Destroy(gameObject);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Weapon.ExplosionRadius);
        }
    }
}
