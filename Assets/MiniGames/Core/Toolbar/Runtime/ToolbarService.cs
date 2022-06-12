using System;

using Core.Services.Toolbar.Views;

using Cysharp.Threading.Tasks.Triggers;

using UnityEngine;

using Object = UnityEngine.Object;

namespace Core.Services.Toolbar
{
    public class ToolbarService
    {
        private readonly ToolbarView _view;

        public ToolbarService()
        {
            var viewPrefab = Resources.Load<ToolbarView>("Toolbar/ToolbarView");
            _view = Object.Instantiate(viewPrefab);
            Hide();
        }

        public void Show(int maxStepsCount, string taskText = " ")
        {
            _view.gameObject.SetActive(true);
            _view.SetMaxAnswerCount(maxStepsCount);
            _view.SetTaskText(taskText);
        }

        public void Fill()
        {
            _view.Fill();
        }

        public void Reset()
        {
            Hide();
            _view.Reset();
        }

        public void SubscribeBackButton(Action action)
        {
            _view.BackButton.onClick.AddListener(action.Invoke);
        }

        private void Hide()
        {
            _view.gameObject.SetActive(false);
        }
    }
}