using UnityEngine;

namespace MiniJameGam9.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/AIInfo", fileName = "AIInfo")]
    public class AIInfo : ScriptableObject
    {
        public float RayMax;
        public float RayStep;
    }
}