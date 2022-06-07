using System;

using DG.Tweening;

using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.Toolbar.Views
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _quitButton;

        private Sequence _sequence;

        public void Show()
        {
            var initScale = _panel.transform.localScale;
            var animInterval = 0.8f;

            _sequence = GetSequence();
            _sequence
                .Append(_panel.transform.DOScale(Vector3.zero, 0))
                .AppendCallback(() => _panel.SetActive(true))
                .Append(_panel.transform.DOScale(initScale * 1.25f, animInterval / 2))
                .Append(_panel.transform.DOScale(initScale, animInterval / 2));
        }

        public void SubscribeRestartButton(Action call)
        {
            _restartButton.onClick.AddListener(call.Invoke);
        }

        public void SubscribeQuitButton(Action call)
        {
            _quitButton.onClick.AddListener(call.Invoke);
        }

        private Sequence GetSequence()
        {
            if (_sequence.IsActive() && _sequence.IsPlaying())
            {
                _sequence.Kill();
                _sequence = null;
            }

            return DOTween.Sequence();
        }
    }
}