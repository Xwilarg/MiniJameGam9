using MiniJameGam9.SO;
using System.IO;
using UnityEngine;

namespace MiniJameGam9.Achievement
{
    public class AchievementManager : MonoBehaviour
    {
        public static AchievementManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private AchievementData[] _achievements;

        private AchievementProgress[] _progress;

        private readonly string _achievementPath = $"{Application.persistentDataPath}/achievement.bin";

        public void Start()
        {
            _progress = new AchievementProgress[_achievements.Length];
            for (int i = 0; i < _progress.Length; i++)
            {
                _progress[i] = new();
            }
            if (Directory.Exists(Application.persistentDataPath))
            {
                if (File.Exists(_achievementPath))
                {
                    using FileStream file = new(_achievementPath, FileMode.Open, FileAccess.Read);
                    using BinaryReader reader = new(file);
                    for (int i = 0; i < _achievements.Length; i++)
                    {
                        _progress[i] = new()
                        {
                            IsAchieved = reader.ReadBoolean(),
                            Progress = reader.ReadInt32()
                        };
                    }
                }
            }
            else
            {
                Directory.CreateDirectory(Application.persistentDataPath);
            }
        }

        public void UpdateKillAchievement(WeaponInfo weapon)
        {
            for (int i = 0; i < _achievements.Length; i++)
            {
                var ach = _achievements[i];
                var prog = _progress[i];
                if (!prog.IsAchieved && ach.Type == AchievementType.Kill && (ach.WeaponConstraint == null || ach.WeaponConstraint == weapon))
                {
                    prog.Progress++;
                    if (prog.Progress >= ach.ValueMax)
                    {
                        prog.IsAchieved = true;
                    }
                }
            }
            UpdateSaves();
        }

        public void UpdateSaves()
        {
            using FileStream file = new(_achievementPath, FileMode.OpenOrCreate, FileAccess.Write);
            using BinaryWriter writer = new(file);
            foreach (var a in _progress)
            {
                writer.Write(a.IsAchieved);
                writer.Write(a.Progress);
            }
        }
    }
}
