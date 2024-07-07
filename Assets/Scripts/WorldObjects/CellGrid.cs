using Core;
using UnityEngine;
using Zenject;

namespace WorldObjects
{
    public class CellGrid : MonoBehaviour, IGameData
    {
        [SerializeField] public BaseCell cellPrefab;
        private ICell[,] _cells;
        private int _size;
        private int _depth;
        
        public void Initialize(int size, int depth)
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
                    var cell = cellObj.GetComponent<ICell>();
                    cell.Initialize(_depth);
                    _cells[i, j] = cell;
                }
            }
        }

        public ICell GetCell(int x, int y)
        {
            return _cells[x, y];
        }

        public void SaveState()
        {
            PlayerPrefs.SetInt(gameObject.name + "_cellSize", _size);
            PlayerPrefs.SetInt(gameObject.name + "_cellDepth", _depth);
            
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    PlayerPrefs.SetInt(gameObject.name + "_dropped_" + i + "_" + j, _cells[i, j].Dropped ? 1 : 0);
                    PlayerPrefs.SetInt(gameObject.name + "_maxDepth_" + i + "_" + j, _cells[i, j].MaxDepth);
                    PlayerPrefs.SetInt(gameObject.name + "_dropDepth_" + i + "_" + j, _cells[i, j].DropDepth);
                    PlayerPrefs.SetInt(gameObject.name + "_currentDepth_" + i + "_" + j, _cells[i, j].CurrentDepth);
                    PlayerPrefs.SetInt(gameObject.name + "hasDrop" + i + "_" + j, _cells[i, j].HasDrop ? 1 : 0);
                }
            }
        } 

        public void LoadState()
        {
            _size = PlayerPrefs.GetInt(gameObject.name + "_cellSize", 10);
            _depth = PlayerPrefs.GetInt(gameObject.name + "_cellDepth", 3);
            _cells = new ICell[_size, _size];
            
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    var position = transform.position;
                    position.x += i * 1.1f;
                    position.z += j * 1.1f;
                    var cellObj = Instantiate(cellPrefab, position, transform.rotation, transform);
                    cellObj.name = "Cell_" + i + "_" + j;
                    var cell = cellObj.GetComponent<ICell>();
                    cell.Dropped = PlayerPrefs.GetInt(gameObject.name + "_dropped_" + i + "_" + j, 0) == 1;
                    cell.MaxDepth = PlayerPrefs.GetInt(gameObject.name + "_maxDepth_" + i + "_" + j, 0);
                    cell.DropDepth = PlayerPrefs.GetInt(gameObject.name + "_dropDepth_" + i + "_" + j, 0);
                    cell.CurrentDepth = PlayerPrefs.GetInt(gameObject.name + "_currentDepth_" + i + "_" + j, 0);
                    cell.HasDrop = PlayerPrefs.GetInt(gameObject.name + "_hasTreasure_" + i + "_" + j, 0) == 1;
                    cell.Initialize();
                    _cells[i, j] = cell;
                }
            }
        }
    }
}