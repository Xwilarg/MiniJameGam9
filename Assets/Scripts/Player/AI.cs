using UnityEngine;
using UnityEngine.AI;

namespace MiniJameGam9.Player
{
    public class AI : MonoBehaviour
    {
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
