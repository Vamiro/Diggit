using System;
using GameInput;
using UnityEngine;

namespace WorldObjects
{
    public class BaseDropSpot : MonoBehaviour, IDropSpot
    {
        public bool ReceiveItem(IDraggable item)
        {
            if (item != null)
            {
                OnReceiveItem?.Invoke();
                return true;
            }
            return false;
        }

        public event Action OnReceiveItem;
    }
}