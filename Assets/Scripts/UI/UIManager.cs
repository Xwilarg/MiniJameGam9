using UnityEngine;

namespace MiniJameGam9.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [SerializeField]
        private GameObject _fragPrefab;

        [SerializeField]
        private Transform _fragContainer;

        private void Awake()
        {
            Instance = this;
        }

        public void ShowFrag(string killer, string killed, Sprite fragIcon, bool amIInside)
        {
            var go = Instantiate(_fragPrefab, _fragContainer);
            go.GetComponent<FragInfo>().Init(killer, killed, fragIcon, amIInside);
        }
    }
}
