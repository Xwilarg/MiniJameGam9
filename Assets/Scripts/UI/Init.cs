using UnityEngine;
using UnityEngine.SceneManagement;

namespace MiniJameGam9.UI
{
    public class Init : MonoBehaviour
    {
        public void Start()
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
