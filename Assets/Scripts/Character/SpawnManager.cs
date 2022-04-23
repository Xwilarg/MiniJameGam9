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
            foreach (var elem in new string[] { /*"Astro", "Zirk"/*, "Gradkal", "Jadith"*/ })
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
            if (p.Camera != null)
            {
                p.Camera.GetComponent<CameraManager>()._follow = ins.transform;
            }
            ins.GetComponentInChildren<ACharacterController>().Profile = p;
            Init(ins, p);
        }

        private void Init(GameObject go, Profile p)
        {
            var characters = GameObject.FindGameObjectsWithTag("Player").Where(x => x.GetInstanceID() != go.GetInstanceID());
            var furthest = _spawnPoints.OrderByDescending(x => Vector3.Distance(x.position,
                characters.OrderBy(y => Vector3.Distance(y.transform.position, x.position)).FirstOrDefault()?.transform?.position ?? Vector3.zero)).First();
            go.transform.position = furthest.position + Vector3.up;

            if (p.Container != null)
            {
                Debug.Log(go.name);
                p.Container._parentController = go.GetComponentInChildren<PlayerController>();
                go.GetComponentInChildren<PlayerController>().Camera = p.Camera;
            }
        }

        public void OnPlayerJoin(PlayerInput value)
        {
            var player = new Profile(false, $"Player {Guid.NewGuid()}", value.camera, value.GetComponent<InputContainer>());
            Init(value.gameObject, player);
            value.GetComponentInChildren<CharacterController>().transform.Translate(Vector3.up);
            value.gameObject.GetComponentInChildren<ACharacterController>().Profile = player;
            _profiles.Add(player);
        }
    }
}
