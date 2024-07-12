using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class PrefsManager: MonoBehaviour
    {
        private readonly List<IGameData> _gameDataObjects = new();
        
        public void RegisterDataObject(IGameData dataObject)
        {
            _gameDataObjects.Add(dataObject);
        }

        public void SaveAll()
        {
            PlayerPrefs.SetInt("SaveGameExists", 1);
            foreach (var dataObject in _gameDataObjects)
            {
                dataObject.SaveState();
            }
        }

        public bool LoadAll()
        {
            //PlayerPrefs.DeleteAll();
            bool saveGameExists = PlayerPrefs.GetInt("SaveGameExists", 0) == 1;
            
            if (saveGameExists)
            {
                foreach (var dataObject in _gameDataObjects)
                {
                    dataObject.LoadState();
                }
            }

            return saveGameExists;
        }

        public void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}