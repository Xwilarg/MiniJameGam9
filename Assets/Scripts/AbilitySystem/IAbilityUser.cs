using Player;
using UnityEngine;

namespace AbilitySystem
{
    /// <summary>
    /// interface IAbilityUser should be implemented on All class that can Use Abilities
    /// </summary>
    public interface IAbilityUser
    {
        // For each Project Fill this Interface with some Field Getter used in "UseCondition"
        // If any Stats should Modify Abilities put them here too
        // It can also be a Good Idea to put a "Target" getter in here
        
        // To have the GameObject on which this is Sitting allow you to Use GetComponent<T>()
        GameObject GameObject { get; }
        public Transform FirePoint { get; }
        Stats Stats { get; }
    }
}
