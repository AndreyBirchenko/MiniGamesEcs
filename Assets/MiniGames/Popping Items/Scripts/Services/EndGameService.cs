using Poppingitems.Views;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Poppingitems.Services
{
    public class EndGameService
    {
        private readonly EndGameView _endGameView;

        public EndGameService(EndGameView endGameView)
        {
            _endGameView = Object.Instantiate(endGameView);
            _endGameView.SubscribeRestartButton(HandleRestartButton);
            _endGameView.SubscribeQuitButton(HandleQuitButton);
        }

        public void ShowEndGamePopup()
        {
            _endGameView.Show();
        }

        private void HandleRestartButton()
        {
            SceneManager.LoadScene(Constants.PoppingItems);
        }

        private void HandleQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}