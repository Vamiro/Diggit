using UnityEngine;
using Zenject;

namespace GameInput
{
    public class DragAndDrop : MonoBehaviour
    {
        public IDraggable CurrentItem { get; private set; }

        [Inject]
        public void Initialize(IInput input)
        {
            input.OnPress += HandlePress;
            input.OnRelease += HandleRelease;
        }

        private void HandlePress(RaycastHit hitInfo)
        {
            var draggable = hitInfo.collider.GetComponent<IDraggable>();
            if (draggable != null)
            {
                StartDrag(draggable);
            }
        }

        private void HandleRelease()
        {
            DropItem();
        }

        private void StartDrag(IDraggable item)
        {
            CurrentItem = item;
            item.Drag();
        }

        private void DropItem()
        {
            if (CurrentItem == null) return;
            CurrentItem.Drop();
            CurrentItem = null;
        }
    }
}