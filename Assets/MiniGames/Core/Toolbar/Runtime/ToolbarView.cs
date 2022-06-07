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
            _slider.transform.position += new Vector3(_stepDistance, 0, 0);
        }
    }
}