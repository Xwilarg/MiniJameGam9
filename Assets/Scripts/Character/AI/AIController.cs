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
        private RaycastHit? _damageTaken; // TODO: Need to forget target when dead
        private float _forgetTimer = -1f;

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

        protected override void OnDamageTaken(Vector3 impactDirection)
        {
            base.OnDamageTaken(impactDirection);
            if (impactDirection != Vector3.zero)
            {
                var dir = new Vector3(impactDirection.x, 0f, impactDirection.z);
                if (DebugManager.Instance.Raycast(
                           id: "" + GetInstanceID() + "damage",
                           origin: transform.position + dir / 2f,
                           direction: dir,
                           color: Color.red,
                           hit: out RaycastHit hit
                           ))
                {
                    _damageTaken = hit;
                    _forgetTimer = 3f;
                }
            }
        }

        private void UpdateDamageSource()
        {
            if (_damageTaken != null)
            {
                var dir = (_damageTaken.Value.point - transform.position).normalized;
                if (DebugManager.Instance.Raycast(
                           id: "" + GetInstanceID() + "vision",
                           origin: transform.position + dir / 2f,
                           direction: dir,
                           color: Color.yellow,
                           hit: out RaycastHit hit
                           ))
                {
                    if (hit.collider.name != _damageTaken.Value.collider.name)
                    {
                        _damageTaken = null;
                    }
                    else
                    {
                        LookAt(_damageTaken.Value.point);
                    }
                }
            }
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

            if (_forgetTimer > 0f)
            {
                _forgetTimer -= Time.deltaTime;
                if (_forgetTimer <= 0f)
                {
                    _damageTaken = null;
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
                    color: Color.blue,
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
                UpdateDamageSource();
            }
            else if (rays.Any(x => x.collider.CompareTag("Player")))
            {
                UpdateBehavior(AIBehavior.Chasing);

                // Look at the closest target
                var closest = rays.Where(x => x.collider.CompareTag("Player")).OrderBy(x => DistanceApprox(transform.position, x.point)).First();
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
                LookAt(closest.point);

                _damageTaken = null;
                Shoot();
            }
            else
            {
                UpdateDamageSource();
            }
        }

        private void LookAt(Vector3 pos)
        {
            _agent.updateRotation = false;
            Vector3 direction = (pos - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z), Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        private Vector2 FlattenY(Vector3 v)
            => new(v.x, v.z);

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
