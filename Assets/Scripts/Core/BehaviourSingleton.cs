using UnityEngine;

namespace Core
{
    public class BehaviourSingleton<T> : MonoBehaviour where T : BehaviourSingleton<T>, new()
    {
        private static T _instance;
        public static T Instance => _instance;

        protected virtual void Awake()
        {
            Debug.Assert(Instance == null);
            _instance = this as T;
        }

        private void OnDestroy()
        {
            if(_instance == this) _instance = null;
        }
    }
}