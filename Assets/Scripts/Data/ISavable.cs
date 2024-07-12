using UnityEngine;

namespace Data
{
        public interface ISavable
        {

                string Id { get; }
    
                void Load(string data);

                string Save();
        }

        public interface ISavable<T> : ISavable {
                void ISavable.Load(string data) => LoadData(string.IsNullOrEmpty(data) ? default : JsonUtility.FromJson<T>(data));
    
                string ISavable.Save() => JsonUtility.ToJson(SaveData());

                void LoadData(T data);
    
                T SaveData();
        }
}