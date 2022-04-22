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

        [SerializeField]
        private Transform _gunOut;

        private int _health;
        protected int _bulletsInMagazine;

        protected void Init()
        {
            _health = _cInfo.BaseHealth;
            _bulletsInMagazine = _baseWeapon.BulletsInMagazine;
        }

        public virtual void Shoot()
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
                    rb.AddForce(forward * _baseWeapon.BulletVelocity + right * _baseWeapon.BulletVelocity * _baseWeapon.BulletDeviation * Random.Range(-1f, 1f), ForceMode.Impulse);
                    rb.useGravity = _baseWeapon.IsAffectedByGravity;
                    go.GetComponent<Bullet>().Damage = _baseWeapon.Damage;
                }
                _bulletsInMagazine -= bulletsShot;
                StartCoroutine(_bulletsInMagazine == 0 ? Reload() : WaitForShootAgain());
            }
        }
        private bool _canShoot = true;

        private IEnumerator Reload()
        {
            yield return new WaitForSeconds(_baseWeapon.ReloadTime);
            _bulletsInMagazine = _baseWeapon.BulletsInMagazine;
            _canShoot = true;
        }

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
    }
}
