using System;
using System.Collections.Generic;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.Toolbar.Views
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _taskTextMeshPro;
        [SerializeField] private Image _slider;
        [SerializeField] private Button _backButton;

        public Button BackButton => _backButton;

        private float _stepFillAmount;

        private Sequence _sequence;

        private void Awake()
        {
            _slider.fillAmount = 0;
        }

        public void SetTaskText(string text)
        {
            _taskTextMeshPro.text = text;
        }

        public void SetMaxAnswerCount(int maxStepsCount)
        {
            _stepFillAmount = 1f / maxStepsCount;
        }

        public void Fill()
        {
            var currentFillAmount = _slider.fillAmount;

            if (currentFillAmount >= 1f)
                return;

            _sequence = GetSequence();
            _sequence
                .Append(_slider.DOFillAmount
                    (currentFillAmount + _stepFillAmount, 0.4f));
        }

        public void Reset()
        {
            _slider.fillAmount = 0;
            GetSequence();
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