using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Data;
using UnityEngine;
using Zenject;

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
        private ICell[,] _cells;
        private int _size;
        private int _depth;

        public void Initialize(int size, int depth, float dropChance)
        {
            if (_cells != null)
            {
                foreach (var cell in _cells)
                {
                    Destroy(cell.GetInstance);
                }
            }

            _size = size;
            _depth = depth;
            _cells = new ICell[_size, _size];

            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var position = transform.position;
                    position.x += i * 1.1f;
                    position.z += j * 1.1f;
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
                    position.x += i * 1.1f;
                    position.z += j * 1.1f;
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