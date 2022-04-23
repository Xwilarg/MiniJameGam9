using MiniJameGam9.Debugging;
using MiniJameGam9.SO;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

namespace MiniJameGam9.Character.AI
{
    public class AIController : ACharacterController
    {
        [SerializeField]
        private AIInfo _info;

        [SerializeField]
        private TMP_Text _debugText;

        private Transform _currentNode;
        private NavMeshAgent _agent;

        private AIBehavior _currBehavior;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            Init();
            UpdateBehavior(AIBehavior.Wandering);
            GetNextNode();
        }

        private void Update()
        {
            if (Vector2.Distance(FlattenY(transform.position), FlattenY(_agent.destination)) < .1f)
            {
                if (_currBehavior != AIBehavior.Wandering) // We lost the player or got our loot, going back at wandering
                {
                    UpdateBehavior(AIBehavior.Wandering);
                    _agent.SetDestination(_currentNode.position);
                }
                else
                {
                    GetNextNode();
                }
            }

            for (var i = -_info.RayMax; i <= _info.RayMax; i += _info.RayStep)
            {
                if (DebugManager.Instance.Raycast(
                    id: "" + GetInstanceID() + i.GetHashCode(),
                    origin: transform.position + transform.forward / 2f,
                    direction: transform.forward + transform.right * i,
                    color: Color.red,
                    hit: out RaycastHit hit
                    ))
                {
                    if (!HaveImprovedWeapon && hit.collider.CompareTag("WeaponCase"))
                    {
                        UpdateBehavior(AIBehavior.Looting);
                        _agent.SetDestination(hit.point);
                        break; // Looting is the most important so we don't need to continue checking
                    }
                    if (hit.collider.CompareTag("Player"))
                    {
                        // We found an enemy, begin the chase
                        UpdateBehavior(AIBehavior.Chasing);
                        if (Vector3.Distance(transform.position, hit.point) < 3f)
                        {
                            // We are already close enough, no point going closer
                            _agent.SetDestination(transform.position);
                        }
                        else
                        {
                            _agent.SetDestination(hit.point);
                        }
                    }
                }
            }

            {
                if (DebugManager.Instance.Raycast(
                        id: "" + GetInstanceID() + "forward",
                        origin: transform.position + transform.forward / 2f,
                        direction: transform.forward,
                        color: Color.blue,
                        hit: out RaycastHit hit
                        ))
                {
                    if (hit.collider.CompareTag("Player")) // Enemy in front of us
                    {
                        Shoot();
                    }
                }
            }
        }

        private Vector2 FlattenY(Vector3 v)
            => new Vector2(v.x, v.z);

        private void GetNextNode()
        {
            _currentNode = AIManager.Instance.NextNode(_currentNode);
            _agent.SetDestination(_currentNode.position);
        }

        private void UpdateBehavior(AIBehavior value)
        {
            _currBehavior = value;
            if (Application.isEditor)
            {
                _debugText.text = $"{_currBehavior}";
            }
        }
    }
}
