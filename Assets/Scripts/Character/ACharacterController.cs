﻿using MiniJameGam9.SO;
using MiniJameGam9.UI;
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

        [SerializeField]
        private Transform _chain;

        [SerializeField]
        private Sprite _defaultDeathIcon;

        [SerializeField]
        private GameObject _hhandgun, _hshotgun, _hsniper, _hgrenade;

        private bool _canMove = true;
        protected virtual void OnCanMoveChange(bool value)
        { }

        public void CanMoveAfterStun()
        {
            StartCoroutine(CanMoveAfterStunE());
        }
        private IEnumerator CanMoveAfterStunE()
        {
            yield return new WaitForSeconds(1f);
            CanMove = true;
        }

        public bool CanMove
        {
            set
            {
                OnCanMoveChange(value);
                _canMove = value;
            }
            get => _canMove;
        }

        private void DisableAll()
        {
            _hhandgun.SetActive(false);
            _hshotgun.SetActive(false);
            _hsniper.SetActive(false);
            _hgrenade.SetActive(false);
        }

        public virtual void OnNewWeapon()
        { }

        protected WeaponInfo CurrentWeapon => _overrideWeapon == null ? _baseWeapon : _overrideWeapon;
        protected bool HaveImprovedWeapon => _overrideWeapon != null;

        public Profile Profile { get; set; }

        private int _health;
        protected int _projectilesInMagazine;
        private bool _canUseChain = true;

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
                int projectilesShot;
                if (!CurrentWeapon.EndlessAmmo)
                {
                    projectilesShot = _projectilesInMagazine >= CurrentWeapon.ProjectileCount ? CurrentWeapon.ProjectileCount : _projectilesInMagazine;
                }
                else
                {
                    projectilesShot = CurrentWeapon.ProjectileCount;
                }
                for (int i = 0; i < projectilesShot; i++)
                {
                    if (CurrentWeapon.ShotEffect != null)
                    {
                        var particleFX = Instantiate(CurrentWeapon.ShotEffect, _gunOut.position, transform.rotation);
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

                    var proj = go.GetComponent<Projectile>();
                    proj.Weapon = CurrentWeapon;
                    proj.Profile = Profile;
                    proj.ShootOrigin = transform.position;
                }
                _projectilesInMagazine -= projectilesShot;
                StartCoroutine(_projectilesInMagazine == 0 && !CurrentWeapon.EndlessAmmo ? Reload() : WaitForShootAgain());
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
                DisableAll();
                _hhandgun.SetActive(true);
                _overrideWeapon = null; // If we have another weapon, we throw it away
                OnNewWeapon();
                _projectilesInMagazine = _baseWeapon.ProjectilesInMagazine; // TODO: Maybe have old amount of projectile before weapon change instead?
            }
            _canShoot = true;
            OnReloadEnd();
        }

        protected virtual void OnReloadEnd()
        { }

        protected virtual void OnDamageTaken(Vector3 impactDirection)
        { }

        private IEnumerator WaitForShootAgain()
        {
            yield return new WaitForSeconds(_baseWeapon.ShotIntervalTime);
            _canShoot = true;
        }

        public bool TakeDamage(int value, Vector3 from, Profile killer, Sprite icon)
        {
            if (_health == 0)
            {
                return false;
            }
            if (value > _health)
            {
                value = _health;
            }
            DamageManager.Instance.AddDamage(Profile, killer, value);
            _health -= value;
            if (_health == 0)
            {
                Profile.Death++;
                var assist = DamageManager.Instance.GetAssist(Profile, killer);
                var inc = "";
                if (killer != null)
                {
                    inc = killer.Name;
                    if (assist != null)
                    {
                        assist.Assist++;
                        inc += $" + {assist.Name}";
                    }
                }
                UIManager.Instance.ShowFrag(inc, Profile.Name, icon, (killer != null && !killer.IsAi) || !Profile.IsAi);
                DamageManager.Instance.AddDeath(Profile);
                Destroy(gameObject);
                SpawnManager.Instance.Spawn(Profile);
                return true;
            }
            OnDamageTaken(from);
            return false;
        }

        public float GetPercentChainTimer => _timerChain / _cInfo.ChainDelay;

        protected void CheckForFallDeath() // Also called each frame
        {
            _timerChain -= Time.deltaTime;
            if (transform.position.y < -10f)
            {
                TakeDamage(1000, Vector3.zero, null, _defaultDeathIcon);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("WeaponCase"))
            {
                _overrideWeapon = other.GetComponent<WeaponCase>().Take();
                DisableAll();
                if (_overrideWeapon.DisplayShotgun) _hshotgun.SetActive(true);
                else if (_overrideWeapon.DisplaySniper) _hsniper.SetActive(true);
                else if (_overrideWeapon.DisplayGrenade) _hgrenade.SetActive(true);
                OnNewWeapon();
                _projectilesInMagazine = CurrentWeapon.ProjectilesInMagazine;
                OnReloadEnd();
                Destroy(other.gameObject);
            }
        }

        public void ThrowChain()
        {
            if (_timerChain <= 0f)
            {
                _canUseChain = false;
                var go = Instantiate(_chain, transform.position + transform.forward, transform.rotation);
                go.GetComponent<Chain>().Caster = transform;
                go.GetComponent<Chain>().Profile = Profile;
                _timerChain = _cInfo.ChainDelay;
            }
        }

        private float _timerChain = 0f;
    }
}
