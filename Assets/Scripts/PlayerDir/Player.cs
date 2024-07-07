using System.Collections.Generic;
using Core;
using WorldObjects;
using UnityEngine;
using Zenject;

namespace PlayerDir
{
    public class Player : MonoBehaviour, IGameData
    {
        public int Shovels { get; private set; }
        public int Bag { get; private set; }
        
        public void Initialize(int initialShovels)
        {
            Shovels = initialShovels;
            Bag = 0;
        }

        public bool UseShovel()
        {
            if (Shovels <= 0) return false;
            Shovels--;
            return true;
        }

        public void CollectItem()
        {
            Bag++;
            Shovels += 5;
        }

        public void SaveState()
        {
            PlayerPrefs.SetInt(gameObject.name + "_shovels", Shovels);
            PlayerPrefs.SetInt(gameObject.name + "_bag", Bag);
        }

        public void LoadState()
        {
            Shovels = PlayerPrefs.GetInt(gameObject.name + "_shovels", 20);
            Bag = PlayerPrefs.GetInt(gameObject.name + "_bag", 0);
        }
    }
}