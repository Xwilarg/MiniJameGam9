using UnityEngine;
using System.Linq;
using MiniJameGam9.Character;

namespace MiniJameGam9.Weapon
{
    public class Chain : MonoBehaviour
    {
        [SerializeField] private string[] _tagsToCheck;

        [SerializeField] private float _speed, _returnSpeed;
        [SerializeField] private float _range, _stopRange;

        public Transform Caster { set; get; }
        private Transform _collidedWith;
        private LineRenderer _line;
        private bool _hasCollided;

        private void Start()
        {
            _line = transform.Find("Line").GetComponent<LineRenderer>();
        }

        private void Update()
        {
            if (Caster)
            {
                _line.SetPosition(0, Caster.position);
                _line.SetPosition(1, transform.position);

                if (_hasCollided)
                {
                    transform.LookAt(Caster);
                    var dist = Vector3.Distance(transform.position, Caster.position);
                    if (dist < _stopRange)
                    {
                        if (_collidedWith != null)
                        {
                            _collidedWith.GetComponent<ACharacterController>().CanMove = false;
                        }
                        Destroy(gameObject);
                    }
                }
                else
                {
                    var dist = Vector3.Distance(transform.position, Caster.position);
                    if (dist > _range)
                        Collision(null);
                }
                transform.Translate(Vector3.forward * _speed * Time.deltaTime);
                
                if (_collidedWith)
                    _collidedWith.transform.position = transform.position;
            }
            else { Destroy(gameObject); }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_hasCollided && _tagsToCheck.Contains(other.gameObject.tag))
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    other.gameObject.GetComponent<ACharacterController>().CanMove = false;
                }
                Collision(other.transform);
            }
        }

        void Collision(Transform col)
        {
            _speed = _returnSpeed;
            _hasCollided = true;
            if (col)
            {
                transform.position = col.position;
                _collidedWith = col;
            }
        }
    }
}
