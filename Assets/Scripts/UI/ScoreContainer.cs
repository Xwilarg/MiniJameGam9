using MiniJameGam9.Character;
using MiniJameGam9.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniJameGam9.UI
{
    public class ScoreContainer : MonoBehaviour
    {
        private void Start()
        {
            foreach (var elem in ScoreManager.Instance.GetAll())
            {
                AddScore(elem.Key, elem.Value);
            }
        }

        [SerializeField]
        private GameObject _scorePrefab;

        public void AddScore(Profile p, Score.ScoreInfo info)
        {
            var go = Instantiate(_scorePrefab, transform);
            var cmp = go.GetComponent<ScoreInfo>();
            cmp.Name.text = p.Name;
            cmp.Score.text = ((int)ScoreManager.Instance.GetScore(info)).ToString();
            cmp.Death.text = info.Death.ToString();
            cmp.Assist.text = info.Assist.ToString();
            cmp.Kill.text = info.Kill.ToString();
            cmp.DamageDealth.text = info.DamageDealt.ToString();
        }

        public void ToMainMenu()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
