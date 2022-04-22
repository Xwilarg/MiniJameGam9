using System.Linq;
using UnityEngine;

namespace MiniJameGam9.Character.AI
{
    public class AIManager : MonoBehaviour
    {
        public static AIManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Transform[] _nodes;

        /// <summary>
        /// Get a random node, excluding the one given in parameter
        /// </summary>
        public Transform NextNode(Transform exclude)
        {
            var eNodeList = _nodes.Where(x => x != exclude).ToArray();
            return eNodeList[Random.Range(0, eNodeList.Length)];
        }
    }
}
