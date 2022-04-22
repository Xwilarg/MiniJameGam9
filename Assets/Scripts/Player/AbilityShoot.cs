using AbilitySystem;
using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "Ability_Shoot", menuName = "Scriptable Object/Abilities/Shoot")]
    public class AbilityShoot : Ability
    {
        [SerializeField] private GameObject Projectile_PF;
        public override void Activate(IAbilityUser user)
        {
            GameObject Projectile = Instantiate(Projectile_PF, user.FirePoint);
            Projectile.transform.position = user.FirePoint.position;
            Projectile.transform.rotation = user.FirePoint.rotation;
            
            Projectile.GetComponent<Rigidbody>().AddForce(user.FirePoint.forward * user.Stats.ProjectileSpeed, ForceMode.Impulse);
        }
    }
}