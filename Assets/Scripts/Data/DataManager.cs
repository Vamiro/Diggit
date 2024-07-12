using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;
using UnityEngine;

namespace Data
{
    public class DataManager : BehaviourSingleton<DataManager>
    {
        private GameData _gameData;
        private List<ISavable> _savableObjects;

        private void Start()
        {
            _savableObjects = FindAllDataPersistenceObjects();
        }

        private List<ISavable> FindAllDataPersistenceObjects()
        {
            IEnumerable<ISavable> objectsData = FindObjectsOfType<MonoBehaviour>().OfType<ISavable>();
            return new List<ISavable>(objectsData);
        }

        public void NewGame()
        {
            this._gameData = new GameData();
        }

        public void LoadGame(string file)
        {
            _savableObjects = FindAllDataPersistenceObjects();
            GameData gameData = Storage.Load<GameData>(file);

            if (gameData.Items == null)
            {
                return;
            }

            var dataMap = gameData.Items.ToDictionary(item => item.Id, item => item.Value);

            foreach (var objectData in _savableObjects)
            {
                objectData.Load(dataMap.TryGetValue(objectData.Id, out var value) ? value : null);
            }
        }

        public void SaveGame(string file)
        {
            _savableObjects = FindAllDataPersistenceObjects();
            var data = new GameData
            {
                Items = _savableObjects
                    .Select(storable => new StoreItem() { Id = storable.Id, Value = storable.Save() }).ToList()
            };
            Storage.Save(data, file);
        }

        public string[] GetSaveList()
        {
            return Storage.Files().Select(Path.GetFileNameWithoutExtension).Where(s => s.StartsWith("Save")).ToArray();
        }

        public void DeleteSave(string fileName)
        {
            Storage.Delete(fileName);
        }
    }
}