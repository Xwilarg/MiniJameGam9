using MiniJameGam9.Character.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

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
        private GameObject _playerPrefab, _playerPrefabContainer, _aiPrefab;

        private readonly List<Profile> _profiles = new();

        private void Start()
        {
            foreach (var elem in new[] { "Astro", "Zirk"/*, "Gradkal", "Jadith"*/ })
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
            var ins = Instantiate(go, Vector3.zero, Quaternion.identity);
            ins.GetComponentInChildren<ACharacterController>().Profile = p;
        }

        private void MoveToSpawn(GameObject go)
        {
            var characters = GameObject.FindGameObjectsWithTag("Player").Where(x => x.gameObject != go);
            var furthest = _spawnPoints.OrderByDescending(x => Vector3.Distance(x.position,
                characters.OrderBy(y => Vector3.Distance(y.transform.position, x.position)).FirstOrDefault()?.transform?.position ?? Vector3.zero)).First();
            go.transform.position = furthest.position + Vector3.up;
        }

        public void OnPlayerJoin(PlayerInput value)
        {
            var player = new Profile(false, $"Player {Guid.NewGuid()}");
            MoveToSpawn(value.transform.parent.gameObject);
            value.gameObject.GetComponentInChildren<ACharacterController>().Profile = player;
            _profiles.Add(player);
        }
    }
}
