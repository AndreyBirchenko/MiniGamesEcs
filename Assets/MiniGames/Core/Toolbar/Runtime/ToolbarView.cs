using DG.Tweening;

using TMPro;

using UnityEngine;

namespace Core.Services.Toolbar.Views
{
    public class ToolbarView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _taskTextMeshPro;
        [SerializeField] private GameObject _slider;

        private float _deltaDistance = 749.5f;
        private float _stepDistance;

        private Sequence _sequence;

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