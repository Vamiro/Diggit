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
        [SerializeField] private TMP_Text InstrumentsText;
        private string _instrumentsText;
        [SerializeField] private TMP_Text BagText;
        private string _bagText;
        [SerializeField] private Button RestartButton;
        [SerializeField] private Button RotateLeftButton;
        [SerializeField] private Button RotateRightButton;
        
        public UnityAction OnRestart;
        public UnityAction OnRotateLeft;
        public UnityAction OnRotateRight;
        

        private void Start()
        {
            _instrumentsText = InstrumentsText.text;
            _bagText = BagText.text;
            RestartButton.onClick.AddListener(() => OnRestart?.Invoke());
            RotateLeftButton.onClick.AddListener(() => OnRotateLeft?.Invoke());
            RotateRightButton.onClick.AddListener(() => OnRotateRight?.Invoke());
        }

        public void SetInstrumentsText(int amount)
        {
            InstrumentsText.SetText(_instrumentsText + " " + amount);
        }
        
        public void SetBagText(int amount)
        {
            BagText.SetText(_bagText + " " + amount);
        }
    }
}