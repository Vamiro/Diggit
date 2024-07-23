using System;
using DG.Tweening;
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
        [SerializeField] private RectTransform _resultScreen;
        private Vector2 _innerPos;
        private Vector2 _outerPos;
        private Vector2 _innerButtonPos;
        private Vector2 _outerButtonPos;

        public UnityAction OnTryAgain;

        protected override void Awake()
        {
            base.Awake();
            RectTransform rectTransform = TryAgainButton.GetComponent<RectTransform>();
            
            TryAgainButton.onClick.AddListener(() =>
            {
                OnTryAgain?.Invoke();
                _resultScreen.anchoredPosition = _outerPos;
                rectTransform.anchoredPosition = _outerButtonPos;
                GetComponent<Image>().DOFade(0f, 0f);
                Hide();
            });
            
            _innerPos = _resultScreen.anchoredPosition;
            _outerPos = _resultScreen.anchoredPosition;
            _outerPos.y += _resultScreen.rect.height + Screen.height;
            _resultScreen.anchoredPosition = _outerPos;

            _innerButtonPos = rectTransform.anchoredPosition;
            _outerButtonPos = _innerButtonPos;
            _outerButtonPos.y += rectTransform.rect.height - Screen.height;
            rectTransform.anchoredPosition = _outerButtonPos;
            
            TryAgainButton.enabled = false;
            GetComponent<Image>().DOFade(0f, 0f);
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

            
            
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_resultScreen.DOAnchorPos(_innerPos, 0.5f))
            .Join(TryAgainButton.GetComponent<RectTransform>().DOAnchorPos(_innerButtonPos, 0.5f))
            .Join(GetComponent<Image>().DOFade(0.7f, 0.5f))
            .OnComplete(() => TryAgainButton.enabled = true)
            .Play();
        }

        protected override void OnHide()
        {
            TryAgainButton.enabled = false;
        }
    }
}