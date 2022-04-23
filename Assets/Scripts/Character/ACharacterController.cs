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

        private int _health;
        protected int _bulletsInMagazine;

        protected void Init()
        {
            _health = _cInfo.BaseHealth;
            _bulletsInMagazine = CurrentWeapon.BulletsInMagazine;
        }

        public virtual bool Shoot()
        {
            if (_canShoot)
            {
                _canShoot = false;
                var bulletsShot = _bulletsInMagazine >= CurrentWeapon.BulletCount ? CurrentWeapon.BulletCount : _bulletsInMagazine;
                for (int i = 0; i < bulletsShot; i++)
                {
                    var go = Instantiate(CurrentWeapon.BulletPrefab, _gunOut.position, Quaternion.identity);
                    var rb = go.GetComponent<Rigidbody>();
                    var forward = (_gunOut.position - transform.position).normalized;
                    var right = Quaternion.AngleAxis(90f, Vector3.up) * forward;
                    rb.AddForce(
                        forward * CurrentWeapon.BulletVelocity + 
                        right * CurrentWeapon.BulletVelocity * CurrentWeapon.ProjectileHorizontalDeviation * Random.Range(-1f, 1f) +
                        Vector3.up * CurrentWeapon.ProjectileVerticalDeviation
                    , ForceMode.Impulse);
                    rb.useGravity = CurrentWeapon.IsAffectedByGravity;
                    go.GetComponent<Bullet>().Damage = CurrentWeapon.Damage;
                }
                _bulletsInMagazine -= bulletsShot;
                StartCoroutine(_bulletsInMagazine == 0 ? Reload() : WaitForShootAgain());
                return true;
            }
            return false;
        }

        public virtual bool Throw()
        {
            if (_canShoot)
            {
                _canShoot = false;
                var bulletsShot = _bulletsInMagazine >= _baseWeapon.BulletCount ? _baseWeapon.BulletCount : _bulletsInMagazine;
                for (int i = 0; i < bulletsShot; i++)
                {
                    var go = Instantiate(_baseWeapon.BulletPrefab, _gunOut.position, Quaternion.identity);
                    var rb = go.GetComponent<Rigidbody>();
                    var forward = (_gunOut.position - transform.position).normalized;
                    var right = Quaternion.AngleAxis(90f, Vector3.up) * forward;
                    rb.AddForce(forward * _baseWeapon.BulletVelocity + right * _baseWeapon.BulletVelocity * _baseWeapon.ProjectileHorizontalDeviation * Random.Range(-1f, 1f), ForceMode.Impulse);
                    rb.useGravity = _baseWeapon.IsAffectedByGravity;
                    go.GetComponent<Bullet>().Damage = _baseWeapon.Damage;
                }
                _bulletsInMagazine -= bulletsShot;
                StartCoroutine(_bulletsInMagazine == 0 ? Reload() : WaitForShootAgain());
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
                _bulletsInMagazine = _baseWeapon.BulletsInMagazine;
            }
            else
            {
                _overrideWeapon = null; // If we have another weapon, we throw it away
                _bulletsInMagazine = _baseWeapon.BulletsInMagazine; // TODO: Maybe have old amount of bullet before weapon change instead?
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

        public void TakeDamage(int value)
        {
            _health -= value;
            if (_health < 0)
            {
                _health = 0;
                Destroy(gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("WeaponCase"))
            {
                _overrideWeapon = other.GetComponent<WeaponCase>().WeaponInfo;
                _bulletsInMagazine = CurrentWeapon.BulletsInMagazine;
                OnReloadEnd();
                Destroy(other.gameObject);
            }
        }
    }
}
