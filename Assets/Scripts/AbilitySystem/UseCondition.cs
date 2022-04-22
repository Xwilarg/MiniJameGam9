using UnityEngine;

namespace MiniJameGam9.AbilitySystem
{
    /// <summary>
    /// ScriptableObject UseCondition describe how the evaluation is done
    /// </summary>
    public abstract class UseCondition : ScriptableObject
    {
        public abstract int getEvaluation(IAbilityUser user);
    }
}