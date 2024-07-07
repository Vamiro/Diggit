using UnityEngine;

namespace WorldObjects
{
    public interface ICell
    {
        GameObject GetInstance { get ; }
        bool Dropped { get; set; }
        bool HasDrop { get; set; }
        int DropDepth { get; set; }
        int CurrentDepth { get; set; }
        int MaxDepth { get; set; }
        void Initialize(int depth);
        void Initialize();
        void Dig();
    }
}