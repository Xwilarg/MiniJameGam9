﻿using MiniJameGam9.Achievement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MiniJameGam9.UI
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Image _blackScreen;

        [SerializeField]
        private DoorUp _door;

        [SerializeField]
        private Transform _achievementContainer;

        [SerializeField]
        private GameObject _achievementPrefab;

        private void Start()
        {
            for (int i = 0; i < AchievementManager.Instance.GetAchievementCount(); i++)
            {
                var go = Instantiate(_achievementPrefab, _achievementContainer);
                AchievementManager.Instance.SetUI(go.GetComponent<AchievementUI>(), i);
            }
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_phase == 1)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 270f, 0f), Quaternion.Euler(0f, 180f, 0f), _timer);
                if (_timer <= 0f)
                {
                    _phase = 2;
                    _timer = 2f;
                }
            }
            else if (_phase == -1)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 90f, 0f), Quaternion.Euler(0f, 180f, 0f), _timer);
                if (_timer <= 0f)
                {
                    _phase = 0;
                }
            }
            else if (_phase == -2)
            {
                Camera.main.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0f, 180f, 0f), Quaternion.Euler(0f, 90f, 0f), _timer);
                if (_timer <= 0f)
                {
                    _phase = 0;
                }
            }
            else if (_phase == 2)
            {
                _blackScreen.color = new(0f, 0f, 0f, (2f - _timer) / 2f);
                Camera.main.transform.Translate(Vector3.forward * Time.deltaTime * 2f);
                if (_timer <= 0f)
                {
                    SceneManager.LoadScene("Basement");
                }
            }
        }

        public void OnClick(InputAction.CallbackContext value)
        {
            if (value.performed && _phase == 0)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit))
                {
                    if (hit.collider.name == "Play")
                    {
                        _phase = 1;
                        _timer = 1f;
                        _door.GoUp = false;
                    }
                    else if (hit.collider.name == "Credits")
                    {
                        _door.GoUp = !_door.GoUp;
                    }
                    else if (hit.collider.name == "Achievements")
                    {
                        _phase = -1;
                        _timer = 1f;
                        _door.GoUp = false;
                    }
                    else if (hit.collider.name == "Menu")
                    {
                        _phase = -2;
                        _timer = 1f;
                    }
                }
            }
        }

        private int _phase = 0;
        private float _timer;
    }
}
