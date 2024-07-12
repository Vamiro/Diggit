using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Core;
using UnityEngine;

namespace WorldObjects
{
    
    
    public class BaseCell : MonoBehaviour, ICell
    {
        public GameObject GetInstance => gameObject;
        
        [SerializeField] public BaseBlock Block;
        [SerializeField] public BaseDrop Drop;
        private IBlock[] _blocks;

        private CellInfo _cellInfo = new();
        public CellInfo CellInfo
        {
            get => _cellInfo;
            set => _cellInfo = value;
        }

        public void Initialize(int depth, float dropChance = 0.2f)
        {
            _blocks = new IBlock[depth];
            _cellInfo.maxDepth = depth;
            _cellInfo.currentDepth = 0;
            _cellInfo.dropped = false;
            
            _cellInfo.hasDrop = UnityEngine.Random.value < dropChance;
            _cellInfo.dropDepth = _cellInfo.hasDrop ? UnityEngine.Random.Range(0, _cellInfo.maxDepth) : -1;
            if(_cellInfo.hasDrop) Debug.Log(gameObject.name + " -- hasDrop = " + _cellInfo.hasDrop + "\ndropDepth = " + _cellInfo.dropDepth);
            
            for (var i = 0; i < _cellInfo.maxDepth; i++)
            {
                var position = transform.position;
                position.y -= i;
                var blockObj = Instantiate(Block.GetInstance, position, Quaternion.identity, transform);
                blockObj.name = "Block_" + i;
                _blocks[i] = blockObj.GetComponent<IBlock>();
            }
        }

        public void Initialize()
        {
            _blocks = new IBlock[_cellInfo.maxDepth];
            
            for (var i = 0; i < _cellInfo.maxDepth; i++)
            {
                if (i >= _cellInfo.currentDepth)
                {
                    var position = transform.position;
                    position.y -= i;
                    var blockObj = Instantiate(Block.GetInstance, position, Quaternion.identity, transform);
                    blockObj.name = "Block_" + i;
                    _blocks[i] = blockObj.GetComponent<IBlock>();
                }
                else _blocks[i] = null;
            }
            if(_cellInfo.dropped) SpawnDrop();
        }

        public void Dig()
        {
            if (_cellInfo.currentDepth < _cellInfo.maxDepth && !_cellInfo.dropped)
            {
                if(_cellInfo.currentDepth == _cellInfo.dropDepth) SpawnDrop();
                _blocks[_cellInfo.currentDepth].Destroy();
                _cellInfo.currentDepth++;
            }
        }

        public void SpawnDrop()
        {
            _cellInfo.dropped = true;
            var position = transform.position;
            position.y -= _cellInfo.dropDepth;
            var dropped = Instantiate(Drop.GetInstance, position, Quaternion.identity, transform);
            dropped.GetComponent<IDrop>().Dropped += () => _cellInfo.dropped = false;
        }
    }
}