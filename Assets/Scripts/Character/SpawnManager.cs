using MiniJameGam9.Character.AI;
using MiniJameGam9.Character.Player;
using MiniJameGam9.Score;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace MiniJameGam9.Character
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            GetComponent<PlayerInputManager>().JoinPlayer(controlScheme: "Keyboard&Mouse");
        }

        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject _playerPrefab, _playerPrefabContainer;

        private readonly List<Profile> _profiles = new();

        public void UploadAllScores()
        {
            foreach (var p in _profiles)
            {
                ScoreManager.Instance.AddProfile(p, new()
                {
                    Kill = p.Kill,
                    Assist = p.Assist,
                    DamageDealt = p.DamageDealt,
                    Death = p.Death
                });
            }
        }

        [SerializeField]
        private GameObject[] _ais;

        private void Start()
        {
            var names = new string[] { "Astro", "Zirk", "Gradkal", "Jadith", "Kulutues", "Timetraveler65" };
            for (int i = 0; i < names.Length; i++)
            {
                var p = new Profile(true, names[i], _ais[i]);
                Spawn(_ais[i], p);
                _profiles.Add(p);
            }
        }

        public void Spawn(Profile p)
            => Spawn(p.Prefab, p);

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

            if (p.Container != null)
            {
                go.GetComponentInChildren<CharacterController>().enabled = false;
            }
            else
            {
                go.GetComponentInChildren<NavMeshAgent>().enabled = false;
            }

            go.transform.position = furthest.position;

            if (p.Container != null)
            {
                var cc = go.GetComponentInChildren<PlayerController>();
                p.Container.ParentController = cc;
                cc.IsKeyboard = p.Container.IsKeyboard;
                cc.Camera = p.Camera;
                cc.Container = p.Container;
                go.GetComponentInChildren<CharacterController>().enabled = true;
            }
            else
            {
                go.GetComponentInChildren<NavMeshAgent>().enabled = true;
            }
        }

        private int _id = 1;
        public void OnPlayerJoin(PlayerInput value)
        {
            var player = new Profile(false, $"Player {_id++}", _playerPrefab, value.camera, value.GetComponent<InputContainer>());
            Init(value.gameObject, player);
            value.gameObject.GetComponentInChildren<ACharacterController>().Profile = player;
            _profiles.Add(player);
        }
    }
}
