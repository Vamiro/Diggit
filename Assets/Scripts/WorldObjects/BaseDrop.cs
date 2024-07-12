using System;
using DG.Tweening;
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
            _dragStartPosition = transform.localPosition;
            transform.localRotation = new Quaternion();
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
                        targetPosition = hit.transform.position + Vector3.up * 1.2f;
                    }
                }
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
                
            }
        }

        public void Drag()
        {
            if (!_isDragging)
            {
                transform.DOScale(2, 1);
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
                        DOTween.KillAll();
                        Dropped?.Invoke();
                        Destroy(gameObject);
                    }
                    return;
                }
            }
            gameObject.layer = LayerMask.NameToLayer("Default");
            DOTween.KillAll();
            transform.localScale = new Vector3(1, 1, 1);
            transform.localPosition = _dragStartPosition;
        }
    }
}