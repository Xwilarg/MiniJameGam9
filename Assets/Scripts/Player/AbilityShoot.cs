using MiniJameGam9.AbilitySystem;
using UnityEngine;

namespace MiniJameGam9.Player
{
    [CreateAssetMenu(fileName = "Ability_Shoot", menuName = "Scriptable Object/Abilities/Shoot")]
    public class AbilityShoot : Ability
    {
        [SerializeField] private GameObject Projectile_PF;

        [SerializeField] private float projSpeed;
        [SerializeField] private int NumberOfProjectiles;
        [SerializeField] private float projDeviation;
        private Vector3 deviation = new Vector3();
        public override void Activate(IAbilityUser user)
        {
            Transform Bullets = GameObject.Find("Bullets").transform;
            deviation = Vector3.zero;
            for (int i = 0; i < NumberOfProjectiles; i++)
            {
                GameObject Projectile = Instantiate(Projectile_PF,Bullets);
                Projectile.transform.position = user.FirePoint.position;
                Projectile.transform.rotation = user.FirePoint.rotation;

                if (i > 0)
                {
                    deviation = new Vector3(Random.Range(-projDeviation, projDeviation), Random.Range(-projDeviation, projDeviation), Random.Range(-projDeviation, projDeviation));
                }
                Projectile.GetComponent<Rigidbody>().AddForce((user.FirePoint.forward + deviation).normalized * projSpeed, ForceMode.Impulse);
            }
        }
    }
}