using System;
using UnityEngine;

namespace AbilitySystem
{
    /// <summary>
    /// class <c>AbilityTimed</c> Abilities that are Used in the Ability Holder.
    /// You should not use this class.
    /// </summary>
    [Serializable]
    public class AbilityTimed
    {
        public Ability Ability;
        [HideInInspector] public float cooldownTime;
        [HideInInspector] public float activeTime;
        [HideInInspector] public AbilityState state;

        public AbilityTimed(Ability _ability)
        {
            Ability = _ability;
            cooldownTime = 0;
            activeTime = 0;
            state = AbilityState.ready;
        }
    }
        
    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }
}