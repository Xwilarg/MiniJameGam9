using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AbilitySystem
{
    ///<summary>
    /// class <c>AbilityHolder</c> Component that Manage the Active and Cooldown Time of the Abilities.
    /// Abilities should be used through the Holder
    /// </summary>
    public class AbilityHolder : MonoBehaviour
    {
        [HideInInspector] public List<AbilityTimed> Abilities = new List<AbilityTimed>();
        [SerializeField] private List<Ability> abilities = new List<Ability>();
        private IAbilityUser owner;

        
        /// <summary>
        /// Return True if any Ability is Active
        /// </summary>
        public bool isUsingAbility()
        {
            return Abilities.Any(at => !at.Ability.StayActive && at.state == AbilityState.active);
        }
        
        /// <summary>
        /// Return All Abilities Registered in this Holder that are Ready To Use
        /// </summary>
        public List<Ability> getReadyAbilities()
        {
            if (isUsingAbility()) return new List<Ability>();
            List<Ability> list = new List<Ability>();
            {
                foreach (AbilityTimed _ability in Abilities)
                {
                    if (_ability.state == AbilityState.ready)
                    {
                        list.Add(_ability.Ability);
                    }
                }
            }

            return list;
        }

        // Used to Register Pre-Existent Abilities
        private void Start()
        {
            owner = GetComponent<IAbilityUser>();
            if(owner == null) Debug.LogError($"Warning this ({gameObject}) don't get any IAbilityUser Component");
            foreach (Ability _ability in abilities)
            {
                Abilities.Add(new AbilityTimed(_ability));
            }
        }

        
        // Used to change the state of abilities after the Delay (activeTime and Cooldown)
        private void Update()
        {
            foreach (AbilityTimed _ability in Abilities)
            {
                switch (_ability.state)
                {
                    case AbilityState.active:
                        if (_ability.activeTime > 0)
                        {
                            _ability.activeTime -= Time.deltaTime;
                        }
                        else if (_ability.Ability.StayActive)
                        { }
                        else
                        {
                            _ability.Ability.BeginCooldown(owner);
                            _ability.cooldownTime = _ability.Ability.getCooldownTime(owner);
                            _ability.state = AbilityState.cooldown;
                        }
                        break;
                    case AbilityState.cooldown:
                        if (_ability.cooldownTime > 0)
                        {
                            _ability.cooldownTime -= Time.deltaTime;
                        }
                        else
                        {
                            _ability.state = AbilityState.ready;
                        }
                        break;
                    case AbilityState.ready:
                        break;
                }
            }
        }
        
        /// <summary>
        /// Method to Use a given Ability (if the Ability is registered in the Holder) Use work only if the State is on Ready
        /// </summary>
        /// <param name="ability">given Ability</param>
        /// <returns>Return if the given Ability was used</returns>
        public bool UseAbility(Ability ability)
        {
            if (owner == null) return false;
            if (abilities.Contains(ability))
            {
                AbilityTimed _ability = Abilities.Find(a => a.Ability == ability);
                if (_ability.state != AbilityState.ready) return false;
                _ability.Ability.Activate(owner);
                _ability.state = AbilityState.active;
                _ability.activeTime = _ability.Ability.getActiveTime(owner);
                return true;
            }

            return false;
        }
        
        /// <summary>
        /// Method Deactivate Ability is use to Disable the passive effect of an Ability tha can StayActive
        /// </summary>
        /// <param name="ability"></param>
        /// <returns></returns>
        private bool DeactivateAbility(Ability ability)
        {
            if (!abilities.Contains(ability)) return false;
            AbilityTimed _ability = Abilities.Find(a => a.Ability == ability);
            if (!_ability.Ability.StayActive || _ability.state != AbilityState.active) return false;
            _ability.Ability.BeginCooldown(owner);
            _ability.cooldownTime = _ability.Ability.getCooldownTime(owner);
            _ability.state = AbilityState.cooldown;
            return true;
        }

        /// <summary>
        /// Method to Register a New Ability in the Holder and make it Accessible to Use
        /// </summary>
        /// <param name="ability">Ability to Add</param>
        public void AddAbility(Ability ability)
        {
            if (abilities.Contains(ability)) return;
            Abilities.Add(new AbilityTimed(ability));
            abilities.Add(ability);
        }
        
        /// <summary>
        /// Method to Unregister a given Ability and make it impossible to use
        /// </summary>
        /// <param name="ability">Ability to Add</param>
        public void RemoveAbility(Ability ability)
        {
            if (!abilities.Contains(ability)) return;
            abilities.Remove(ability);
            AbilityTimed AT = Abilities.Find(at => at.Ability == ability);
            Abilities.Remove(AT);
        }

        /// <summary>
        /// Method Used to set a given Registered Ability to Ready.
        /// </summary>
        /// <param name="ability"></param>
        public void SetAbilityReady(Ability ability)
        {
            if (!abilities.Contains(ability)) return;
            AbilityTimed _ability = Abilities.Find(a => a.Ability == ability);
            _ability.state = AbilityState.ready;
        }
    }
}