using System.Collections.Generic;
using UnityEngine;

namespace MiniJameGam9.AbilitySystem
{
    /// <summary>
    /// ScriptableObject AbilityConditioned Hold an Ability and a List of UseCondition
    /// </summary>
    [CreateAssetMenu(fileName = "ABILITY_NAME_Conditioned", menuName = "Scriptable Object/Ability/Ability with Condition", order = 0)]
    public class AbilityConditioned : ScriptableObject
    {
        [SerializeField] private Ability ability;
        [SerializeField] private List<UseCondition> conditions = new List<UseCondition>();
        public Ability Ability => ability;

        /// <summary>
        /// Return the Current Evaluation Value for this Ability
        /// </summary>
        /// <param name="user">the Owner of the Ability</param>
        public int getEvaluation(IAbilityUser user)
        {
            int value = 0;
            foreach (UseCondition condition in conditions)
            {
                value += condition.getEvaluation(user);
            }

            return value;
        }
    }
}