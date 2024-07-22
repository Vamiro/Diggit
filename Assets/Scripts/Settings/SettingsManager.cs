using System;
using Core;
using Data;
using UnityEngine;
using UnityEngine.Audio;

namespace Settings
{
    [Serializable]
    public class SettingsData
    {
        public bool IsMuted;
    }
    
    public class SettingsManager: BehaviourSingleton<SettingsManager>, ISavable<SettingsData>
    {
        public event Action<bool> OnMute;
        public AudioMixer AudioMixer { get; set; }
        
        private bool _isMuted;
        public bool IsMuted
        {
            get => _isMuted;
            set
            {
                if (AudioMixer)
                {
                    if (value)
                    {
                        AudioMixer.SetFloat("Master", -80f);
                    }
                    else
                    {
                        AudioMixer.SetFloat("Master", 0f);
                    }

                    _isMuted = value;
                    OnMute?.Invoke(true);
                }
            }
        }
        
        private string _id = "SettingsData";
        public string Id => _id;
        public void LoadData(SettingsData data)
        {
            IsMuted = data.IsMuted;
        }

        public SettingsData SaveData()
        {
            return new SettingsData
            {
                IsMuted = _isMuted,
            };
        }
    }
}