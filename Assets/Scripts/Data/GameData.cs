using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public List<StoreItem> Items = new();
    }

    [Serializable]
    public struct StoreItem
    {
        public string Id;
        public string Value;
    }
}