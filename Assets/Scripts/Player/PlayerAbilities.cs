using System;
using System.Collections.Generic;
using AbilitySystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerAbilities : MonoBehaviour, IAbilityUser, PlayerControls.IAbilitiesActions
    {
        public GameObject GameObject => this.gameObject;
        private PlayerControls controls;

        [SerializeField] private Transform firePoint;
        public Transform FirePoint => firePoint;
        [SerializeField] private Stats stats;
        public Stats Stats => stats;

        [SerializeField] private AbilityHolder holder;

        [SerializeField] private List<Ability> Abilities = new List<Ability>();

        private bool isShooting;
        private void Start()
        {
            controls = new PlayerControls();
            controls.Abilities.SetCallbacks(this);
            controls.Abilities.Enable();

            foreach (Ability _ability in Abilities)
            {
                holder.AddAbility(_ability);
            }
        }

        private void Update()
        {
            if (isShooting)
            {
                holder.UseAbility(Abilities.Find(a => a.Name == "Shoot"));
            }
        }


        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                isShooting = true;

            if (context.canceled)
                isShooting = false;
        }
    }
}