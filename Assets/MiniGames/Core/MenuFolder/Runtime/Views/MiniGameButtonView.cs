using System;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Client.Views
{
    public class MiniGameButtonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Button _button;

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        public void SubscribeButton(Action action)
        {
            _button.onClick.AddListener(action.Invoke);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}