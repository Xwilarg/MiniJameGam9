using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Extension;

namespace AbilitySystem
{
    /// <summary>
    /// class <c>AutoUseAbility</c> make the Owner an IA that Use the Abilities Automatically
    /// if you Use this Class you have to Fill the Abilities here and not in the AbilityHolder
    /// </summary>
    [RequireComponent(typeof(AbilityHolder))]
    public class AutoUseAbility : MonoBehaviour
    {
        [SerializeField] private List<AbilityConditioned> Abilities = new List<AbilityConditioned>();
        private AbilityHolder AbilityHolder;
        private IAbilityUser user;
        private Ability nextToUse;
        private bool nextIsReady;
        private float tick;

        /// <summary>
        /// Return True if any Ability is Active
        /// </summary>
        public bool isUsingAbility => AbilityHolder.isUsingAbility();
        
        // Get the User and Holder from attached component
        // then Fill the Abilities List form the Holder
        private void Awake()
        {
            user = GetComponent<IAbilityUser>();
            if(user == null) Debug.LogError($"Warning this ({gameObject}) don't get any IAbilityUser Component");
            AbilityHolder = GetComponent<AbilityHolder>();
            if(AbilityHolder == null) Debug.LogError($"Warning this ({gameObject}) don't get any AbilityHolder Component");
            foreach (AbilityConditioned ability in Abilities)
            {
                AbilityHolder.AddAbility(ability.Ability);
            }
        }
        
        private void Update()
        {
            if (tick > 0)
                tick -= Time.deltaTime;
            else
            {
                SlowUpdate();
                tick = 0.1f;
            }
        }
        
        /// <summary>
        /// if the Owner is not currently Using an Ability Find and Use the Best One
        /// </summary>
        private void SlowUpdate()
        {
            if (Abilities.Count == 0) return;
            if (AbilityHolder.isUsingAbility()) return;
            if (!nextIsReady)
            {
                FindNextAbilityToUse();
            }
            if (nextIsReady)
                UseAbility();
        }

        /// <summary>
        /// Method <code>UseAbility</code> tell the Ability Holder to Use the nextAbility
        /// </summary>
        private void UseAbility()
        {
            if (AbilityHolder.UseAbility(nextToUse))
            {
                nextIsReady = false;
                nextToUse = null;
            }
        }

        /// <summary>
        /// Method <c>FindNextAbilityToUse</c> search and evaluate the best Ability to Use
        /// in the current situation
        /// </summary>
        private void FindNextAbilityToUse()
        {
            int bestEvaluation = 0;
            foreach (AbilityConditioned _ability in getAbilitiesReady())
            {
                int currentEval = _ability.getEvaluation(user);
                if (currentEval > bestEvaluation || bestEvaluation == 0)
                {
                    bestEvaluation = currentEval;
                    nextToUse = _ability.Ability;
                }
            }
            nextIsReady = bestEvaluation > 0;
        }

        /// <summary>
        /// Return a List of Abilities that are ReadyToUse
        /// </summary>
        private List<AbilityConditioned> getAbilitiesReady()
        {
            List<Ability> ReadyToUse = AbilityHolder.getReadyAbilities();
            List<AbilityConditioned> list = new List<AbilityConditioned>();
            foreach (AbilityConditioned _ability in Abilities)
            {
                if (ReadyToUse.Contains(_ability.Ability))
                {
                    list.Add(_ability);
                }
            }

            return list;
        }

        /// <summary>
        /// Method AddAbility Add an AbilityConditioned to the pool of usable Abilities
        /// </summary>
        /// <param name="abilityConditioned"></param>
        public void AddAbility(AbilityConditioned abilityConditioned)
        {
            Abilities.Add(abilityConditioned);
            AbilityHolder.AddAbility(abilityConditioned.Ability);
        }
    }
}