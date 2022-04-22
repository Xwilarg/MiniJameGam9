using System.Collections.Generic;
using MiniJameGam9.AbilitySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniJameGam9.Player
{
    public class PlayerAbilities : MonoBehaviour, IAbilityUser
    {
        public GameObject GameObject => this.gameObject;

        [SerializeField] private Transform firePoint;
        public Transform FirePoint => firePoint;
        [SerializeField] private Stats stats;
        public Stats Stats => stats;

        [SerializeField] private AbilityHolder holder;

        [SerializeField] private Ability _shoot;

        private bool isShooting;

        private void Update()
        {
            if (isShooting)
            {
                holder.UseAbility(_shoot);
            }
        }

        
        public void OnShoot(InputAction.CallbackContext context)
        {
            if (context.performed)
                isShooting = true;

            if (context.canceled)
                isShooting = false;
        }

    }
}