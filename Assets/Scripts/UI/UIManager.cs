using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text ShovelText;
        [SerializeField] private TMP_Text GoldText;
        [SerializeField] private Button RestartButton;
        
        public UnityAction OnRestart;

        private void Start()
        {
            RestartButton.onClick.AddListener(() => OnRestart?.Invoke());
        }

        public void SetShavelText(int amount)
        {
            ShovelText.SetText("SHAVELS: " + amount);
        }
        
        public void SetGoldText(int amount)
        {
            GoldText.SetText("GOLD: " + amount);
        }
    }
}