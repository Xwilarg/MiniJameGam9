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
                if (_currBehavior == AIBehavior.Chasing) // We lost the player, going back at wandering
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
                    if (hit.collider.CompareTag("Player"))
                    {
                        // We found an enemy, begin the chase
                        UpdateBehavior(AIBehavior.Chasing);
                        _agent.SetDestination(hit.point);
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
