using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniJameGam9.UI
{
    public class FragInfo : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _killer, _killed;

        [SerializeField]
        private Image _fragIcon, _background;

        public void Init(string killerName, string killedName, Sprite icon, bool amIInside)
        {
            _killer.text = killerName;
            _killed.text = killedName;
            _fragIcon.sprite = icon;

            _background.color = amIInside ? Color.white : Color.grey;

            Destroy(gameObject, amIInside ? 5f : 2f);
        }
    }
}
