using System;

using DG.Tweening;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Core.Services.Toolbar.Views
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _taskTextMeshPro;
        [SerializeField] private GameObject _slider;
        [SerializeField] private Button _backButton;

        public Button BackButton => _backButton;

        private float _deltaDistance = 2532;
        private float _stepDistance;
        private Vector3 _initPosition;

        private Sequence _sequence;

        private void Awake()
        {
            _initPosition = _slider.transform.position;
        }

        public void SetTaskText(string text)
        {
            _taskTextMeshPro.text = text;
        }

        public void SetMaxAnswerCount(int maxStepsCount)
        {
            _stepDistance = _deltaDistance / maxStepsCount;
        }

        public void Fill()
        {
            _sequence = GetSequence();
            _sequence
                .Append(_slider.transform
                    .DOMoveX(_slider.transform.position.x + _stepDistance, 0.4f));
        }

        public void Reset()
        {
            _slider.transform.position = _initPosition;
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