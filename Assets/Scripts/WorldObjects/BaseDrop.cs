using System;
using GameInput;
using UnityEngine;

namespace WorldObjects
{
    public class BaseDrop : MonoBehaviour, IDraggable, IDrop
    {
        public GameObject GetInstance => gameObject;
        public event Action Dropped;

        private Vector3 _dragStartPosition;
        private Camera _mainCamera;
        private bool _isDragging;
        private const float DistanceFromCamera = 5f;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (_isDragging)
            {
                var mousePosition = Input.mousePosition;
                mousePosition.z = DistanceFromCamera;
                var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(mousePosition);
                var targetPosition = mouseWorldPosition;
                
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    var dropSpot = hit.collider.GetComponent<IDropSpot>();
                    if (dropSpot != null)
                    {
                        targetPosition = hit.transform.position + Vector3.up;
                    }
                }
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
                
            }
            else
            {
                var rotation = transform.rotation;
                rotation = Quaternion.Lerp(rotation, Quaternion.Inverse(rotation),
                    Time.deltaTime * 10f);
                transform.rotation = rotation;
            }
        }

        public void Drag()
        {
            if (!_isDragging)
            {
                _dragStartPosition = transform.position;
                _isDragging = true;
                gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            }
        }

        public void Drop()
        {
            _isDragging = false;
            
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit))
            {
                var dropSpot = hit.collider.GetComponent<IDropSpot>();
                if (dropSpot != null)
                {
                    if (dropSpot.ReceiveItem(this))
                    {
                        Dropped?.Invoke();
                        Destroy(gameObject);
                    }
                    return;
                }
            }
            gameObject.layer = LayerMask.NameToLayer("Default");
            transform.position = _dragStartPosition;
        }
    }
}