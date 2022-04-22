using UnityEngine;

namespace AbilitySystem
{
    /// <summary>
    /// Scriptable Object <c>Ability</c> is the 
    /// </summary>
    public abstract class Ability : ScriptableObject
    {
        [Tooltip("Never Used to search Ability So Double Name are OK")]
        [SerializeField] private string abilityName;

        [Tooltip("For the Ability Holder, in seconds")]
        [SerializeField] private float cooldownTime;
        
        [Tooltip("For the Ability Holder, in seconds")]
        [SerializeField] private float activeTime;
        
        [Tooltip("if Stay Active is true, this Ability will never be on Cooldown (example Passive or Aura Abilities)")]
        [SerializeField] private bool stayActive; 
        
        [SerializeField] private AbilityType type; 
        
        public string Name => abilityName;
        
        /// <summary>
        /// return the CooldownTime of this Ability, (it can be modified by the user)
        /// </summary>
        /// <param name="user">the Ability User that Use the Skill</param>
        public virtual float getCooldownTime(IAbilityUser user)
        {
            return cooldownTime;
        }
        
        /// <summary>
        /// return the ActiveTime of this Ability, (it can be modified by the user)
        /// </summary>
        /// <param name="user">the Ability User that Use the Skill</param>
        public virtual float getActiveTime(IAbilityUser user) 
        {
            return activeTime;
        }

        public AbilityType Type => type;
        
        public bool StayActive => stayActive;

        public abstract void Activate(IAbilityUser user);
        
        public virtual void BeginCooldown(IAbilityUser user){}
    }
}