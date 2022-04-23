using UnityEngine;
using System.Collections;
using MiniJameGam9.Character;

namespace MiniJameGam9.Environment
{
    public class SpikesTrap : MonoBehaviour
    {
        [SerializeField]private Material _defaultMat;
        [SerializeField]private Material _triggeredMat;
        [SerializeField]private int _damages = 100;
        [SerializeField]private float _resetTime = 2f;

        [SerializeField]
        private Sprite _trapIcon;

        private bool _isTriggered = false;

        private void Start()
        {
            GetComponent<Renderer>().material = _defaultMat;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isTriggered)
                return;

            _isTriggered = true;
            GetComponent<Renderer>().material = _triggeredMat;
            StartCoroutine(ResetTrap());

            if (other.GetComponent<ACharacterController>() != null)
                other.GetComponent<ACharacterController>().TakeDamage(_damages, Vector3.zero, null, _trapIcon);
        }

        private IEnumerator ResetTrap()
        {
            yield return new WaitForSeconds(_resetTime);
            _isTriggered = false;
            GetComponent<Renderer>().material = _defaultMat;
        }
    }
}
