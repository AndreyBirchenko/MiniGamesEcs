using System;

using Core.Services.Toolbar.Views;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Core.Services.Toolbar
{
    public class EndGameService
    {
        private readonly EndGameView _endGameView;

        public EndGameService()
        {
            var endGamePrefab = Resources.Load<EndGameView>("EndGame/EndGameView");
            _endGameView = Object.Instantiate(endGamePrefab);
        }

        public void ShowEndGamePopup()
        {
            _endGameView.Show();
        }

        public void HideEndGamePopup()
        {
            _endGameView.Hide();
        }

        public void SubscribeRestartButton(Action action)
        {
            _endGameView.SubscribeRestartButton(action.Invoke);
        }

        public void SubscribeQuitButton(Action action)
        {
            _endGameView.SubscribeQuitButton(action.Invoke);
        }
    }
}