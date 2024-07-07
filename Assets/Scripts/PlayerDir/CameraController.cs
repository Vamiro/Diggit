using System;
using UnityEngine;

namespace PlayerDir
{
    public class CameraController : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        
    }
}