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
        [SerializeField] private GameObject _slider;
        [SerializeField] private Button _backButton;

        public Button BackButton => _backButton;

        private float _deltaDistance = 2532;
        private int _stepIndex;
        private Vector3 _initPosition;
        private List<Vector3> _positions;

        private Sequence _sequence;

        private void Awake()
        {
            _initPosition = _slider.transform.position;
            _positions = new List<Vector3>();
        }

        public void SetTaskText(string text)
        {
            _taskTextMeshPro.text = text;
        }

        public void SetMaxAnswerCount(int maxStepsCount)
        {
            var stepDistance = _deltaDistance / maxStepsCount;
            var position = new Vector3(_slider.transform.position.x, _slider.transform.position.y, 0);
                
            for (int i = 0; i < maxStepsCount; i++)
            {
                position = new Vector3(position.x + stepDistance, position.y, 0);
                _positions.Add(position);
            }
        }

        public void Fill()
        {
            if(_stepIndex >= _positions.Count)
                return;
            
            _sequence = GetSequence();
            _sequence
                .Append(_slider.transform
                    .DOMoveX(_positions[_stepIndex++].x, 0.4f));
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