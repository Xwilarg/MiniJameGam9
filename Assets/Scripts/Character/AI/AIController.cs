using MiniJameGam9.Debugging;
using MiniJameGam9.SO;
using System.Collections.Generic;
using System.Linq;
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

        private float DistanceApprox(Vector3 a, Vector3 b)
        {
            return Mathf.Pow(b.x - a.x, 2) + Mathf.Pow(b.y - a.y, 2);
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

            // Register hits
            List<RaycastHit> rays = new();
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
                    rays.Add(hit);
                }
            }

            // Choose next behavior
            _agent.updateRotation = true;

            // Looting weapon if we are still with the base one is our priority
            if (!HaveImprovedWeapon && rays.Any(x => x.collider.CompareTag("WeaponCase")))
            {
                UpdateBehavior(AIBehavior.Looting);
                _agent.SetDestination(rays.First(x => x.collider.CompareTag("WeaponCase")).point);
            }
            else if (rays.Any(x => x.collider.CompareTag("Player")))
            {
                UpdateBehavior(AIBehavior.Chasing);

                // Look at the closest target
                var closest = rays.Where(x => x.collider.CompareTag("Player")).OrderBy(x => DistanceApprox(transform.position, x.point)).First();
                _agent.updateRotation = false;
                if (Vector3.Distance(transform.position, closest.point) < 10f)
                {
                    // We are already close enough, no point going closer
                    _agent.SetDestination(transform.position);
                }
                else
                {
                    _agent.SetDestination(closest.point);
                }

                // We keep looking at the target
                Vector3 direction = (closest.point - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

                Shoot();
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
