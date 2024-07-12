using System;
using System.Collections.Generic;
using Core;
using Data;
using WorldObjects;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace PlayerDir
{
    [Serializable]
    public class PlayerData
    {
        public int Instruments;
        public int Bag;
        public int Record;
    }
    
    public class Player : MonoBehaviour, ISavable<PlayerData>
    {
        public int Record { get; private set; }
        public int Instruments { get; private set; }
        public int Bag { get; private set; }
        
        public event Action OnInstrumentsEnded;
        
        public void Initialize(int initialShovels)
        {
            Instruments = initialShovels;
            Bag = 0;
        }

        public void UseShovel()
        {
            Instruments--;
            
            if (Instruments <= 0)
            {
                OnInstrumentsEnded?.Invoke();
            }
        }

        public void CollectItem()
        {
            Bag++;
            Instruments += 5;
        }

        public void UpdateRecord()
        {
            if (Record < Bag)
            {
                Record = Bag;
            }
        }

        //[SerializeField] private string _id = Guid.NewGuid().ToString();
        [SerializeField] private string _id = "PlayerSaveData";
        public string Id => _id;
        
        public void LoadData(PlayerData data)
        {
            Instruments = data.Instruments;
            Bag = data.Bag;
            Record = data.Record;
        }

        public PlayerData SaveData()
        {
            return new PlayerData()
            {
                Instruments = this.Instruments,
                Bag = this.Bag,
                Record = this.Record,
            };
        }
    }
}