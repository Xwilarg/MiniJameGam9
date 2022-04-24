using UnityEngine;

namespace MiniJameGam9.Weapon
{
    internal class Rotate : MonoBehaviour
    {
        private void Update()
        {
            transform.Rotate(new Vector3(0f, Time.deltaTime, 0f));
        }
    }
}
