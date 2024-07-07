using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Core;
using UnityEngine;

namespace WorldObjects
{
    public class BaseCell : MonoBehaviour, ICell
    {
        [SerializeField] public BaseBlock Block;
        [SerializeField] public BaseDrop Drop;

        private IBlock[] _blocks;

        public GameObject GetInstance => gameObject;
        public bool Dropped { get; set; }
        public int MaxDepth { get; set; }
        public int DropDepth { get; set; }
        public int CurrentDepth { get; set;  }
        public bool HasDrop { get; set; }
        
        public void Initialize(int depth)
        {
            _blocks = new IBlock[depth];
            MaxDepth = depth;
            CurrentDepth = 0;
            Dropped = false;
            
            HasDrop = UnityEngine.Random.value < 0.2f;
            DropDepth = HasDrop ? UnityEngine.Random.Range(0, MaxDepth) : -1;
            if(HasDrop) Debug.Log(gameObject.name + " -- HasDrop = " + HasDrop + "\nDropDepth = " + DropDepth);
            
            for (var i = 0; i < MaxDepth; i++)
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
            _blocks = new IBlock[MaxDepth];
            
            for (var i = 0; i < MaxDepth; i++)
            {
                if (i >= CurrentDepth)
                {
                    var position = transform.position;
                    position.y -= i;
                    var blockObj = Instantiate(Block.GetInstance, position, Quaternion.identity, transform);
                    blockObj.name = "Block_" + i;
                    _blocks[i] = blockObj.GetComponent<IBlock>();
                }
                else _blocks[i] = null;
            }
            if(Dropped) SpawnDrop();
        }

        public void Dig()
        {
            if (CurrentDepth < MaxDepth && !Dropped)
            {
                if(CurrentDepth == DropDepth) SpawnDrop();
                _blocks[CurrentDepth].Destroy();
                CurrentDepth++;
            }
        }

        public void SpawnDrop()
        {
            Dropped = true;
            var position = transform.position;
            position.y -= DropDepth;
            var dropped = Instantiate(Drop.GetInstance, position, Quaternion.identity, transform);
            dropped.GetComponent<IDrop>().Dropped += () => Dropped = false;
        }
    }
}