using System;
using UnityEngine;

namespace MiniJameGam9.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]
        [Range(0f, 10f)]
        [Tooltip("Speed of the player")]
        public float ForceMultiplier = 1f;

        [Tooltip("Gravity multiplier to make the player fall")]
        public float GravityMultiplicator;
    }
}