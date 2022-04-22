using UnityEngine;

namespace MiniJameGam9.Enemies
{
    [CreateAssetMenu(fileName = "Enemy_Stats", menuName = "Scriptable Object/Enemy/Stats")]
    public class EnemyStatsSO : ScriptableObject
    {
        [SerializeField] private int _HP;
        public int HP => _HP;
        [SerializeField] private float _moveSpeed;
        public float MoveSpeed => _moveSpeed;
    }
}