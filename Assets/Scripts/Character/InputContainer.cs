using MiniJameGam9.Character.Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MiniJameGam9.Character
{
    public class InputContainer : MonoBehaviour
    {
        public PlayerController ParentController { set; get; }
        public TMP_Text AmmoText;

        public bool IsKeyboard => GetComponent<PlayerInput>().currentControlScheme == "Keyboard&Mouse";

        public void OnMovement(InputAction.CallbackContext value)
        {
            if (ParentController != null)
                ParentController.OnMovement(value);
        }

        public void OnLook(InputAction.CallbackContext value)
        {
            if (ParentController != null)
                ParentController.OnLook(value);
        }

        public void OnShoot(InputAction.CallbackContext value)
        {
            if (ParentController != null)
                ParentController.OnShoot(value);
        }

        public void OnChain(InputAction.CallbackContext value)
        {
            if (ParentController != null)
                ParentController.OnChain(value);
        }
    }
}
