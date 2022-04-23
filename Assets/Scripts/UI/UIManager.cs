using TMPro;
using UnityEngine;

namespace MiniJameGam9.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public TMP_Text AmmoDisplay;
    }
}
