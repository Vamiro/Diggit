using System;
using Unity.VisualScripting;
using UnityEngine;

namespace GameInput
{
    public interface IInput
    {
        event Action<RaycastHit> OnPress;
        event Action OnRelease;

        void UpdateInput();
    }
}