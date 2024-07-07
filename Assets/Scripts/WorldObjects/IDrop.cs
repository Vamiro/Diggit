using System;
using UnityEngine;

namespace WorldObjects
{
    public interface IDrop
    {
        public GameObject GetInstance { get; }

        public event Action Dropped;
    }
}