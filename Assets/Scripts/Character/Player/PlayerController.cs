using MiniJameGam9.Debugging;
using MiniJameGam9.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniJameGam9.Character.Player
{
    public class PlayerController : ACharacterController
    {
        [SerializeField]
        private PlayerInfo _info;

        public Camera Camera { set; get; }
        public bool IsKeyboard { set; get; }
        public InputContainer Container { set; get; }
        private CharacterController _cc;
        private Vector3 _mov;
        private float _verticalSpeed;
        private Vector2 _mousePos;

        private LineRenderer _lr;
        private int _ignoreLayer;

        private void Start()
        {
            Init();
            _cc = GetComponent<CharacterController>();
            _lr = GetComponent<LineRenderer>();
            _ignoreLayer = 1 << 7;
            _ignoreLayer |= 1 << 8;
            _ignoreLayer = ~_ignoreLayer;
            UpdateUI();
        }

        private void Update()
        {
            _lr.enabled = CanMove && !CurrentWeapon.IsAffectedByGravity;

            if (_lr.enabled)
            {
                Vector3 outPos;
                if (DebugManager.Instance.Raycast(
                              id: "" + GetInstanceID() + "damage",
                              origin: transform.position + transform.forward / 2f,
                              direction: transform.forward,
                              color: Color.green,
                              hit: out RaycastHit hit,
                              layer: _ignoreLayer
                              ))
                {
                    outPos = hit.point;
                }
                else
                {
                    outPos = transform.position + transform.forward * 100f;
                }
                _lr.SetPositions(new[]
                {
                    transform.position,
                    outPos
                });
            }

            CheckForFallDeath();
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                return;
            }

            Vector3 desiredMove = new(_mov.x, 0f, _mov.y);

            // Get a normal for the surface that is being touched to move along it
            Physics.SphereCast(transform.position, _cc.radius, Vector3.down, out RaycastHit hitInfo,
                               _cc.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            Vector3 moveDir = Vector3.zero;
            moveDir.x = desiredMove.x * _info.ForceMultiplier;
            moveDir.z = desiredMove.z * _info.ForceMultiplier;

            if (_cc.isGrounded && _verticalSpeed < 0f) // We are on the ground and not jumping
            {
                moveDir.y = -.1f; // Stick to the ground
                _verticalSpeed = -_info.GravityMultiplicator;
            }
            else
            {
                // We are currently jumping, reduce our jump velocity by gravity and apply it
                _verticalSpeed += Physics.gravity.y * _info.GravityMultiplicator;
                moveDir.y += _verticalSpeed;
            }

            _cc.Move(moveDir);

            if (IsKeyboard)
            {
                Vector3 mouseWorld = Camera.ScreenToWorldPoint(new Vector3(_mousePos.x, _mousePos.y, Vector3.Distance(transform.position, Camera.transform.position)));
                Vector3 forward = mouseWorld - transform.position;
                var rot = Quaternion.LookRotation(forward, Vector3.up);
                transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
            }
            else
            {
                if (_mousePos != Vector2.zero)
                {
                    Vector3 forward = new(_mousePos.x, 0f, _mousePos.y);
                    var rot = Quaternion.LookRotation(forward, Vector3.up);
                    transform.rotation = Quaternion.Euler(0f, rot.eulerAngles.y, 0f);
                }
            }
        }

        private void UpdateUI()
        {
            Container.AmmoText.text = $"{_projectilesInMagazine}";
        }

        public override bool Shoot()
        {
            var result = base.Shoot();
            if (result)
            {
                UpdateUI();
                Camera.GetComponent<CameraManager>().Launch(.1f, CurrentWeapon.ShakeIntensity);
            }
            return result;
        }

        protected override void OnReloadEnd()
        {
            base.OnReloadEnd();
            UpdateUI();
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            _mov = value.ReadValue<Vector2>().normalized;
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            _mousePos = value.ReadValue<Vector2>();
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            if (value.performed && CanMove)
            {
                Shoot();
            }
        }

        public void OnChain(InputAction.CallbackContext value)
        {
            if (value.performed && CanMove)
                ThrowChain();
        }
    }
}