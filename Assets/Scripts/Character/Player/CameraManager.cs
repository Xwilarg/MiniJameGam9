using UnityEngine;

namespace MiniJameGam9.Character.Player
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private Transform _playerTr;
        private Vector3 _offset;

        void Start()
        {
            _offset = transform.position - _playerTr.position;
        }

        void Update()
        {
            transform.position = _playerTr.position + _offset;
        }
    }

}