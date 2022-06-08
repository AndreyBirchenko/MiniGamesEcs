using Core.Services.Toolbar.Views;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Services.Toolbar
{
    public class EndGameService
    {
        private readonly EndGameView _endGameView;

        public EndGameService()
        {
            var endGamePrefab = Resources.Load<EndGameView>("EndGame/EndGameView");
            _endGameView = Object.Instantiate(endGamePrefab);
            _endGameView.SubscribeRestartButton(HandleRestartButton);
            _endGameView.SubscribeQuitButton(HandleQuitButton);
        }

        public void ShowEndGamePopup()
        {
            _endGameView.Show();
        }

        private void HandleRestartButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void HandleQuitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}