using Core.Services.Toolbar.Views;

using UnityEngine;

namespace Core.Services.Toolbar
{
    public class ToolbarService
    {
        private readonly ToolbarView _view;

        public ToolbarService()
        {
            var viewPrefab = Resources.Load<ToolbarView>("Toolbar/ToolbarView");
            _view = Object.Instantiate(viewPrefab);
        }

        public void Show(int maxStepsCount, string taskText = " ")
        {
            _view.SetMaxAnswerCount(maxStepsCount);
            _view.SetTaskText(taskText);
        }

        public void Fill()
        {
            _view.Fill();
        }
    }
}