using Unity.VisualScripting;
using UnityEngine;

namespace WorldObjects
{
    public interface IBlock
    {
        GameObject GetInstance { get ; } 
        void Destroy();
    }
}