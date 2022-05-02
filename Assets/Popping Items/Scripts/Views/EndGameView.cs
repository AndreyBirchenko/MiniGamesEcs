using System;

using UnityEngine;
using UnityEngine.UI;

namespace Poppingitems.Views
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _quitButton;
        
        public void Show()
        {
            _panel.SetActive(true);
        }

        public void SubscribeRestartButton(Action call)
        {
            _restartButton.onClick.AddListener(call.Invoke);
        }
        
        public void SubscribeQuitButton(Action call)
        {
            _quitButton.onClick.AddListener(call.Invoke);
        }
    }
}