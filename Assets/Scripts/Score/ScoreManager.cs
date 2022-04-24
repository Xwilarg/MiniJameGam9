using MiniJameGam9.Character;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Score
{
    public class ScoreManager : MonoBehaviour
    {
        private static ScoreManager _instance;
        public static ScoreManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("Score Manager", typeof(ScoreManager));
                    DontDestroyOnLoad(go);
                    _instance = go.AddComponent<ScoreManager>();
                }
                return _instance;
            }
        }

        public void ClearAll()
        {
            _scores.Clear();
        }

        public void AddProfile(Profile p, ScoreInfo info)
        {
            _scores.Add(p, info);
        }

        public float GetScore(ScoreInfo info)
            => info.Kill + info.Assist / 4f;

        public IOrderedEnumerable<KeyValuePair<Profile, ScoreInfo>> GetAll()
        {
            return _scores.OrderByDescending(x => GetScore(x.Value));
        }

        private Dictionary<Profile, ScoreInfo> _scores = new();
    }
}
