using UnityEngine;
using System.Linq;
using MiniJameGam9.Character;
using MiniJameGam9.Achievement;

namespace MiniJameGam9.Weapon
{
    public class Chain : MonoBehaviour
    {
        [SerializeField] private string[] _tagsToCheck;

        [SerializeField] private float _speed, _returnSpeed;
        [SerializeField] private float _range, _stopRange;

        [SerializeField]
        private Sprite _grappinIcon;

        public Transform Caster { set; get; }
        public Profile Profile { set; get; }
        private Transform _collidedWith;
        private LineRenderer _line;
        private bool _hasCollided;

        private void Start()
        {
            _line = transform.Find("Line").GetComponent<LineRenderer>();
        }

        private void OnDestroy()
        {
            if (otherACC != null)
            {
                otherACC.CanMove = true;
            }
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
                        if (otherACC != null)
                        {
                            otherACC.CanMove = true;
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
            if (!_hasCollided)
            {
                if (_tagsToCheck.Contains(other.gameObject.tag))
                {
                    if (other.gameObject.CompareTag("Player"))
                    {
                        otherACC = other.gameObject.GetComponent<ACharacterController>();
                        otherACC.CanMove = false;
                        AchievementManager.Instance.UpdateHookAchievement();
                        if (otherACC.TakeDamage(10, transform.position, Profile, _grappinIcon))
                        {
                            AchievementManager.Instance.HookKill();
                            Collision(null);
                        }
                        else
                        {
                            Collision(other.transform);
                        }
                    }
                    else
                    {
                        Collision(other.transform);
                    }
                }
                else
                {
                    Collision(null);
                }
            }
        }
        private ACharacterController otherACC = null;

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
