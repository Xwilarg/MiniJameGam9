using MiniJameGam9.Character;
using MiniJameGam9.Score;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniJameGam9.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField]
        private GameObject _fragPrefab;

        [SerializeField]
        private Transform _fragContainer;

        [SerializeField]
        private TMP_Text _timerManager;

        private float _timer = .25f * 60f;

        private void Awake()
        {
            Instance = this;
        }

        private string ToTwoDigits(int nb)
        {
            if (nb < 10)
            {
                return "0" + nb;
            }
            return nb.ToString();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            _timerManager.text = $"{(int)(_timer / 60)}:{ToTwoDigits((int)(_timer % 60))}";
            if (_timer <= 0f)
            {
                ScoreManager.Instance.ClearAll();
                SpawnManager.Instance.UploadAllScores();
                SceneManager.LoadScene("GameOver");
            }
        }

        public void ShowFrag(string killer, string killed, Sprite fragIcon, bool amIInside)
        {
            var go = Instantiate(_fragPrefab, _fragContainer);
            go.GetComponent<FragInfo>().Init(killer, killed, fragIcon, amIInside);
        }
    }
}
