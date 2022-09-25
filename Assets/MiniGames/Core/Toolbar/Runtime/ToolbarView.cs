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
        private float _destinationFillAmount;

        private Tween _fillTween;

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
            _destinationFillAmount += _stepFillAmount;

            if (_slider.fillAmount >= 1f)
                return;

            _fillTween?.Kill();
            _fillTween = _slider.DOFillAmount(_destinationFillAmount, 0.4f);
        }

        public void Reset()
        {
            _slider.fillAmount = 0;
            _destinationFillAmount = 0;
            _fillTween?.Kill();
        }
    }
}