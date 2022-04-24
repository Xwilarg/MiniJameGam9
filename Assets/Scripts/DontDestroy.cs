using UnityEngine;

namespace MiniJameGam9
{
    public class DontDestroy : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
