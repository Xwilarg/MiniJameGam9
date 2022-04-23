using MiniJameGam9.Character.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniJameGam9.Character
{
    public class InputContainer : MonoBehaviour
    {
        public PlayerController _parentController;

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (_parentController != null)
                _parentController.OnMovement(value);
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            if (_parentController != null)
                _parentController.OnLook(value);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            if (_parentController != null)
                _parentController.OnShoot(value);
        }
    }
}
