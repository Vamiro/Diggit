using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace WorldObjects
{
    public class BaseBlock : MonoBehaviour, IBlock
    {
        public GameObject GetInstance => gameObject;

        private void Start()
        {
            var mat = GetComponent<MeshRenderer>();
            mat.material.mainTextureOffset = new Vector2(Random.Range(0f, 10f), Random.Range(0f, 10f));
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}