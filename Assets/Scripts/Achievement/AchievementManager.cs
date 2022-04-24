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
            DontDestroyOnLoad(this);

            _achievementPath = $"{Application.persistentDataPath}/achievement.bin";
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

        [SerializeField]
        private AchievementData[] _achievements;

        private AchievementProgress[] _progress;

        private string _achievementPath;

        public int GetAchievementCount() => _achievements.Length;

        public void SetUI(AchievementUI ui, int i)
        {
            var ach = _achievements[i];
            var prog = _progress[i];
            if (!prog.IsAchieved)
            {
                ui.Image.color = Color.black;
            }
            ui.Title.text = ach.Name + (prog.IsAchieved ? "" : $" ({prog.Progress} / {ach.ValueMax})");
            ui.Description.text = GetDescription(ach);
        }

        private string GetDescription(AchievementData data)
        {
            return data.Type switch
            {
                AchievementType.Kill =>
                    data.WeaponConstraint == null ?
                    $"Kill a total of {data.ValueMax} enemies" :
                    $"Kill a total of {data.ValueMax} enemies with a {data.WeaponConstraint.name}",
                AchievementType.Win => $"Win a total one game",
                AchievementType.GrappingHook => $"Grab a total of {data.ValueMax} enemies with your chain",
                AchievementType.HookKill => $"Kill a total of {data.ValueMax} enemies with your chain",
                _ => ""
            };
        }

        private void Progress(int i)
        {
            var ach = _achievements[i];
            var prog = _progress[i];
            prog.Progress++;
            if (prog.Progress >= ach.ValueMax)
            {
                prog.IsAchieved = true;
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
                    Progress(i);
                }
            }
            UpdateSaves();
        }

        public void UpdateHookAchievement()
        {
            for (int i = 0; i < _achievements.Length; i++)
            {
                var ach = _achievements[i];
                var prog = _progress[i];
                if (!prog.IsAchieved && ach.Type == AchievementType.GrappingHook)
                {
                    Progress(i);
                }
            }
            UpdateSaves();
        }

        public void Win()
        {
            for (int i = 0; i < _achievements.Length; i++)
            {
                var ach = _achievements[i];
                var prog = _progress[i];
                if (!prog.IsAchieved && ach.Type == AchievementType.Win)
                {
                    Progress(i);
                }
            }
            UpdateSaves();
        }

        public void HookKill()
        {
            for (int i = 0; i < _achievements.Length; i++)
            {
                var ach = _achievements[i];
                var prog = _progress[i];
                if (!prog.IsAchieved && ach.Type == AchievementType.HookKill)
                {
                    Progress(i);
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
