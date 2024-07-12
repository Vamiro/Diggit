using System;
using UnityEngine;

namespace WorldObjects
{
    [Serializable]
    public class CellInfo
    {
        public bool dropped;
        public bool hasDrop;
        public int dropDepth;
        public int currentDepth;
        public int maxDepth;
    }
    
    public interface ICell
    {
        GameObject GetInstance { get; }
        CellInfo CellInfo { get; set; }
        void Initialize(int depth, float dropChance);
        void Initialize();
        void Dig();
    }
}