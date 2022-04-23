using MiniJameGam9.SO;
using System;

namespace MiniJameGam9.Achievement
{
    [Serializable]
    public class AchievementData
    {
        public string Name;
        public AchievementType Type;
        public bool IsPersistentBetweenRounds;
        public int ValueMax;

        public WeaponInfo WeaponConstraint;
    }
}
