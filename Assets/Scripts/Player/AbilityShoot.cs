using MiniJameGam9.AbilitySystem;
using UnityEngine;

namespace MiniJameGam9.Player
{
    [CreateAssetMenu(fileName = "Ability_Shoot", menuName = "Scriptable Object/Abilities/Shoot")]
    public class AbilityShoot : Ability
    {
        [SerializeField] private GameObject _projectile_PF;

        [SerializeField] private float _projSpeed;
        [SerializeField] private int _numberOfProjectiles;
        [SerializeField] private float _projDeviation;
        private Vector3 _deviation = new Vector3();
        public override void Activate(IAbilityUser user)
        {
            
            Debug.Log($"{name} : fire");
            Transform Bullets = GameObject.Find("Bullets").transform;
            _deviation = Vector3.zero;
            for (int i = 0; i < _numberOfProjectiles; i++)
            {
                GameObject Projectile = Instantiate(_projectile_PF,Bullets);
                Projectile.transform.position = user.FirePoint.position;
                Projectile.transform.rotation = user.FirePoint.rotation;
                
                _deviation = new Vector3(Random.Range(-_projDeviation, _projDeviation), Random.Range(-_projDeviation, _projDeviation), Random.Range(-_projDeviation, _projDeviation));
                
                Projectile.GetComponent<Rigidbody>().AddForce((user.FirePoint.forward + _deviation).normalized * _projSpeed, ForceMode.Impulse);
            }
        }
    }
}