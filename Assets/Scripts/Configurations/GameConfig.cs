using UnityEngine;

namespace Configurations
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public int GridSize;
        public int GridDepth;
        [Range(0f, 1f)]
        public float DropChance;
        public int ShovelsAmount;
    }
}