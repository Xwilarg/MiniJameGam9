using UnityEngine;

namespace MiniJameGam9.UI
{
    public class DoorUp : MonoBehaviour
    {
        public bool GoUp { set; get; } = false;

        private void Update()
        {
            if (GoUp && transform.position.y < 2f)
            {
                transform.Translate(transform.up * Time.deltaTime);
            }
            else if (!GoUp && transform.position.y > 0f)
            {
                transform.Translate(-transform.up * Time.deltaTime);
                if (transform.position.y < 0f)
                {
                    transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
                }
            }
        }
    }
}
