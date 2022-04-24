using MiniJameGam9.Character;
using System.Collections.Generic;
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
                    _instance = go.AddComponent<ScoreManager>();
                }
                return _instance;
            }
        }

        private Dictionary<Profile, ScoreInfo> _scores = new();
    }
}
