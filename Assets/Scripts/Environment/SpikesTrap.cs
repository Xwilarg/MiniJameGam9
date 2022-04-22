using UnityEngine;
using MiniJameGam9.Character;
using MiniJameGam9.Character.Player;

namespace MiniJameGam9.Environment
{
    public class SpikesTrap : MonoBehaviour
    {
        [SerializeField]private Material _defaultMat;
        [SerializeField]private Material _triggeredMat;
        [SerializeField]private int _damages;

        private bool _isTriggered = false;

        private void Start()
        {
            GetComponent<Renderer>().material = _defaultMat;
        }

        private void OnTriggerEnter(Collider other)
        {
            Trigger(other);
        }

        private void Trigger(Collider other)
        {
            if (_isTriggered)
                return;

            _isTriggered = true;
            GetComponent<Renderer>().material = _triggeredMat;
            Invoke("Reset", 2);
            other.GetComponent<ACharacterController>().TakeDamage(_damages);
        }

        private void Reset()
        {
            _isTriggered = false;
            GetComponent<Renderer>().material = _defaultMat;
        }
    }
}
