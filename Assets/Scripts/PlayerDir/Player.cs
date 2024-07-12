using System;
using System.Collections.Generic;
using Core;
using Data;
using WorldObjects;
using UnityEngine;
using Zenject;

namespace PlayerDir
{
    [Serializable]
    public class PlayerData
    {
        public int Shovels;
        public int Bag;
    }
    
    public class Player : MonoBehaviour, ISavable<PlayerData>
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

        //[SerializeField] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private string _id = "PlayerSaveData";
        public string Id => _id;
        
        public void LoadData(PlayerData data)
        {
            Shovels = data.Shovels;
            Bag = data.Bag;
        }

        public PlayerData SaveData()
        {
            return new PlayerData()
            {
                Shovels = this.Shovels,
                Bag = this.Bag,
            };
        }
    }
}