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

        [Tooltip("Damage the player will take per projectile")]
        public int Damage;

        [Tooltip("Screenshake intensity when shooting")]
        public float ShakeIntensity;

        [Header("Projectile info")]

        [Tooltip("Is the projectile affected by gravity")]
        public bool IsAffectedByGravity;

        [Tooltip("Prefab used for the projectile")]
        public GameObject ProjectilePrefab;

        [Tooltip("Number of projectiless shot at once (used by shotguns, else 1)")]
        public int ProjectileCount;

        [Tooltip("Number of projectiles in magazine, when it's empty the player need to reload, reduced by ProjectileCount at each shot")]
        public int ProjectilesInMagazine;

        [Tooltip("Force applied to the projectile when launched")]
        public float ProjectileVelocity;

        [Tooltip("Horizontal spread of the projectile, aka their deviation angle")]
        public float HorizontalDeviation;

        [Tooltip("Vertical spread of the projectile, aka their deviation angle")]
        public float VerticalDeviation;

        [Tooltip("Radius that determines which targets are hit by explosive projectiles")]
        public float ExplosionRadius;

        [Tooltip("The time it takes for the projectile to explode after hitting a target")]
        public float TimeBeforeExplode;

    }
}