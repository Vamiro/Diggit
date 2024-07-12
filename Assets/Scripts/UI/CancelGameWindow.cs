using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class CancelGameWindow : BaseWindow
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Button TryAgainButton;

        public UnityAction OnTryAgain;

        protected override void Awake()
        {
            base.Awake();
            TryAgainButton.onClick.AddListener(() =>
            {
                OnTryAgain?.Invoke();
                Hide();
            });
        }

        protected override void OnShow(object[] args)
        {
            if ((int)args[0] > (int)args[1])
            {
                _text.text = "Congratulations!\n";
                _text.text += "Old record: " + args[1] + "!\n";
                _text.text += "New record: " + args[0] + "!\n";
            }
            else
            {
                _text.text = "Failed\n";
                _text.text += "Record: " + args[1] + "\n"
                              + "Current try: " + args[0] + "\n";
            }
        }

        protected override void OnHide()
        {
            
        }
    }
}