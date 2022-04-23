using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Character
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject _playerPrefab, _aiPrefab;

        private void Start()
        {
            Spawn(_playerPrefab);
            foreach (var elem in new[] { "Astro", "Zirk" })
            {
                Spawn(_aiPrefab);
            }
        }

        private void Spawn(GameObject go)
        {
            var characters = GameObject.FindGameObjectsWithTag("Player");
            var furthest = _spawnPoints.OrderByDescending(x => Vector3.Distance(x.position, characters.OrderBy(y => Vector3.Distance(y.transform.position, x.position)).First().transform.position)).First();
            Instantiate(go, furthest.position, Quaternion.identity);
        }
    }
}
