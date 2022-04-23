using UnityEngine;

namespace MiniJameGam9.Character
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _spawnPoints;

        [SerializeField]
        private GameObject _playerPrefab, _aiPrefab;

        private void Start()
        {
            
        }
    }
}
