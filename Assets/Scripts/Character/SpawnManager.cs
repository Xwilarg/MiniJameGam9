using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Character
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject _playerPrefab, _aiPrefab;

        private readonly List<Profile> _profiles = new();

        private void Start()
        {
            var player = new Profile(false, "Player");
            Spawn(_playerPrefab, player);
            _profiles.Add(player);
            foreach (var elem in new[] { "Astro"/*, "Zirk" */ })
            {
                var p = new Profile(true, elem);
                Spawn(_aiPrefab, p);
                _profiles.Add(p);
            }
        }

        public void Spawn(Profile p)
            => Spawn(p.IsAi ? _aiPrefab : _playerPrefab, p);

        private void Spawn(GameObject go, Profile p)
        {
            var characters = GameObject.FindGameObjectsWithTag("Player");
            var furthest = _spawnPoints.OrderByDescending(x => Vector3.Distance(x.position,
                characters.OrderBy(y => Vector3.Distance(y.transform.position, x.position)).FirstOrDefault()?.transform?.position ?? Vector3.zero)).First();
            var ins = Instantiate(go, furthest.position + Vector3.up, Quaternion.identity);
            ins.GetComponent<ACharacterController>().Profile = p;
        }
    }
}
