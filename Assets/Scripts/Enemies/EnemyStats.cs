using System;

namespace MiniJameGam9.Enemies
{
    [Serializable]
    public class EnemyStats
    {
        public int HP = 0;
        public float MoveSpeed = 0;
        public EnemyStats(EnemyStatsSO so)
        {
            HP = so.HP;
            MoveSpeed = so.MoveSpeed;
        }
    }
}