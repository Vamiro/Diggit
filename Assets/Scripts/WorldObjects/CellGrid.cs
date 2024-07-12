using System;
using System.Collections.Generic;
using Data;
using UnityEngine;
using DG.Tweening;


namespace WorldObjects
{
    [Serializable]
    public class CellGridData
    {
        public List<CellInfo> Cells;
        public int CellSize;
        public int CellDepth;
    }

    public class CellGrid : MonoBehaviour, ISavable<CellGridData>
    {
        [SerializeField] public BaseCell cellPrefab;
        [SerializeField] private float _padding = 1.02f;
        private ICell[,] _cells;
        private int _size;
        private int _depth;
        private bool _isRotating;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Start()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        public void Initialize(int size, int depth, float dropChance)
        {
            if (_cells != null)
            {
                foreach (var cell in _cells)
                {
                    Destroy(cell.GetInstance);
                }

                transform.position = _initialPosition;
                transform.rotation = _initialRotation;
            }

            _size = size;
            _depth = depth;
            _cells = new ICell[_size, _size];

            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var position = transform.position;
                    position.x += (i + 0.5f) * _padding - _size / 2f * _padding;
                    position.z += (j + 0.5f) * _padding - _size / 2f * _padding;
                    var cellObj = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                    cellObj.name = "Cell_" + i + "_" + j;
                    _cells[i, j] = cellObj.GetComponent<ICell>();
                    _cells[i, j].Initialize(_depth, dropChance);
                }
            }
        }

        public ICell GetCell(int x, int y)
        {
            return _cells[x, y];
        }

        public bool CheckDrop()
        {
            foreach (var cell in _cells)
            {
                if (cell.CellInfo.dropped) return true;
            }

            return false;
        }

        public void RotateGrid(float angle)
        {
            if (!_isRotating)
            {
                _isRotating = true;
                var rot = transform.rotation;
                rot *= Quaternion.Euler(0, angle, 0);
                transform.DORotateQuaternion(rot, 0.5f)
                    .SetEase(Ease.InOutCubic)
                    .OnComplete(() =>
                    {
                        transform.rotation = rot;
                        _isRotating = false;
                    });
            }
        }

        private void CalcGridCenter()
        {
            Vector3 sumVector = new Vector3(0f, 0f, 0f);

            foreach (Transform child in transform)
            {
                sumVector += child.position;
            }

            var gridCenter = sumVector / transform.childCount;
        }

        //[SerializeField] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private string _id = "GridCellSaveData";
        public string Id => _id;

        public void LoadData(CellGridData data)
        {
            _size = data.CellSize;
            _depth = data.CellDepth;
            _cells = new ICell[_size, _size];
            int count = 0;
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var position = transform.position;
                    position.x += (i + 0.5f) * _padding - _size / 2f * _padding;
                    position.z += (j + 0.5f) * _padding - _size / 2f * _padding;
                    var cellObj = Instantiate(cellPrefab, position, transform.rotation, transform);
                    cellObj.name = "Cell_" + i + "_" + j;
                    _cells[i, j] = cellObj.GetComponent<ICell>();
                    _cells[i, j].CellInfo = data.Cells[count];
                    count++;
                    _cells[i, j].Initialize();
                }
            }
        }

        public CellGridData SaveData()
        {
            var cellGridData = new CellGridData()
            {
                CellSize = _size,
                CellDepth = _depth,
                Cells = new List<CellInfo>(),
            };

            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    cellGridData.Cells.Add(_cells[i, j].CellInfo);
                }
            }

            return cellGridData;
        }
    }
}