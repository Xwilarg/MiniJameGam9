using MiniJameGam9.SO;
using MiniJameGam9.Weapon;
using System.Collections;
using UnityEngine;

namespace MiniJameGam9.Character
{
    public abstract class ACharacterController : MonoBehaviour
    {
        [SerializeField]
        private SO.CharacterInfo _cInfo;

        [SerializeField]
        private WeaponInfo _baseWeapon;

        private WeaponInfo _overrideWeapon;

        [SerializeField]
        private Transform _gunOut;

        protected WeaponInfo CurrentWeapon => _overrideWeapon == null ? _baseWeapon : _overrideWeapon;
        protected bool HaveImprovedWeapon => _overrideWeapon != null;

        public Profile Profile { get; set; }

        private int _health;
        protected int _projectilesInMagazine;

        protected void Init()
        {
            _health = _cInfo.BaseHealth;
            _projectilesInMagazine = CurrentWeapon.ProjectilesInMagazine;
        }

        public virtual bool Shoot()
        {
            if (_canShoot)
            {
                _canShoot = false;
                var projectilesShot = _projectilesInMagazine >= CurrentWeapon.ProjectileCount ? CurrentWeapon.ProjectileCount : _projectilesInMagazine;
                for (int i = 0; i < projectilesShot; i++)
                {
                    if (CurrentWeapon.ShotParticleEffect != null)
                    {
                        var particleFX = Instantiate(CurrentWeapon.ShotParticleEffect, _gunOut.position, transform.rotation);
                        particleFX.GetComponent<ParticleSystem>().Play();
                        Destroy(particleFX, 1f);
                    }
                    var go = Instantiate(CurrentWeapon.ProjectilePrefab, _gunOut.position, Quaternion.identity);
                    var rb = go.GetComponent<Rigidbody>();
                    var forward = (_gunOut.position - transform.position).normalized;
                    var right = Quaternion.AngleAxis(90f, Vector3.up) * forward;
                    rb.AddForce(
                        forward * CurrentWeapon.ProjectileVelocity + 
                        right * CurrentWeapon.ProjectileVelocity * CurrentWeapon.HorizontalDeviation * Random.Range(-1f, 1f) +
                        Vector3.up * CurrentWeapon.VerticalDeviation
                    , ForceMode.Impulse);
                    rb.useGravity = CurrentWeapon.IsAffectedByGravity;
                    
                    if (null != go.GetComponent<Projectile>())
                    {
                        go.GetComponent<Projectile>().Weapon = CurrentWeapon;
                        go.GetComponent<Projectile>().Author = this;
                    }
                }
                _projectilesInMagazine -= projectilesShot;
                StartCoroutine(_projectilesInMagazine == 0 ? Reload() : WaitForShootAgain());
                return true;
            }
            return false;
        }

        private bool _canShoot = true;

        private IEnumerator Reload()
        {
            if (_overrideWeapon == null)
            {
                yield return new WaitForSeconds(_baseWeapon.ReloadTime);
                _projectilesInMagazine = _baseWeapon.ProjectilesInMagazine;
            }
            else
            {
                _overrideWeapon = null; // If we have another weapon, we throw it away
                _projectilesInMagazine = _baseWeapon.ProjectilesInMagazine; // TODO: Maybe have old amount of projectile before weapon change instead?
            }
            _canShoot = true;
            OnReloadEnd();
        }

        protected virtual void OnReloadEnd()
        { }

        private IEnumerator WaitForShootAgain()
        {
            yield return new WaitForSeconds(_baseWeapon.ShotIntervalTime);
            _canShoot = true;
        }

        public bool TakeDamage(int value)
        {
            _health -= value;
            if (_health < 0)
            {
                _health = 0;
                Profile.Death++;
                Destroy(gameObject);
                SpawnManager.Instance.Spawn(Profile);
                return true;
            }
            return false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WeaponCase"))
            {
                _overrideWeapon = other.GetComponent<WeaponCase>().Take();
                _projectilesInMagazine = CurrentWeapon.ProjectilesInMagazine;
                OnReloadEnd();
                Destroy(other.gameObject);
            }
        }
    }
}
