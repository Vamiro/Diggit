using System;
using UnityEngine;
using Input = UnityEngine.Windows.Input;

namespace GameInput
{
    public class DesktopInput : MonoBehaviour, IInput
    {
        public event Action<RaycastHit> OnPress;
        public event Action OnRelease;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        public void UpdateInput()
        {
            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                var ray = _mainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out var hitInfo))
                {
                    OnPress?.Invoke(hitInfo);
                }
            }

            if (UnityEngine.Input.GetMouseButtonUp(0))
            {
                OnRelease?.Invoke();
            }
        }
    }
}