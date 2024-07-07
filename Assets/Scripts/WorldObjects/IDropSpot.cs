using System;
using GameInput;

namespace WorldObjects
{
    public interface IDropSpot
    {
        bool ReceiveItem(IDraggable item);
        public event Action OnReceiveItem;
    }
}