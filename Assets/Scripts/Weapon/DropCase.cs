using System.Collections;
using UnityEngine;

namespace MiniJameGam9.Weapon
{
    public class DropCase : MonoBehaviour
    {
        [SerializeField]
        private GameObject Prefab;

        private void Start()
        {
            Respawn();
        }

        public void WaitAndRespawn()
        {
            StartCoroutine(WaitAndRespawnInternal());
        }

        private IEnumerator WaitAndRespawnInternal()
        {
            yield return new WaitForSeconds(5f);
            Respawn();
        }

        private void Respawn()
        {
            var go = Instantiate(Prefab, transform.position + Vector3.up, Quaternion.identity);
            go.GetComponent<WeaponCase>().Parent = this;
        }
    }
}
