using UnityEngine;

namespace MiniJameGam9.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/WeaponInfo", fileName = "WeaponInfo")]
    public class WeaponInfo : ScriptableObject
    {
        [Header("Weapon info")]

        [Tooltip("Time in second for the reload")]
        public float ReloadTime;

        [Tooltip("Time in second between each shot")]
        public float ShotIntervalTime;

        [Tooltip("Damage the player will take per bullet")]
        public int Damage;

        [Tooltip("Screenshake intensity when shooting")]
        public float ShakeIntensity;

        [Tooltip("Prefab for particle when weapon is shot")]
        public GameObject ParticleEffect;

        [Header("Bullet info")]

        [Tooltip("Number of bullets shot at once (used by shotguns, else 1)")]
        public int BulletCount;

        [Tooltip("Spread of the bullet, aka their deviation angle")]
        public float BulletDeviation;

        [Tooltip("Number of bullets in magazine, when it's empty the player need to reload, reduced by BulletCount at each shot")]
        public int BulletsInMagazine;

        [Tooltip("Force applied to the bullet when launched")]
        public float BulletVelocity;

        [Tooltip("Prefab used for the bullet")]
        public GameObject BulletPrefab;

        [Tooltip("Is the bullet affected by gravity")]
        public bool IsAffectedByGravity;
    }
}