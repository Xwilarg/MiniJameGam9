using MiniJameGam9.Debugging;
using MiniJameGam9.SO;
using UnityEngine;
using UnityEngine.AI;

namespace MiniJameGam9.Player
{
    public class AI : MonoBehaviour
    {
        [SerializeField]
        private AIInfo _info;

        private Transform _currentNode;
        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            GetNextNode();
        }

        private void Update()
        {
            if (Vector2.Distance(FlattenY(transform.position), FlattenY(_currentNode.position)) < .1f)
            {
                GetNextNode();
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
    }
}
