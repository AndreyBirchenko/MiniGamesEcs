using Core.Services.Toolbar.Views;

using UnityEngine;

namespace Core.Services.Toolbar
{
    public class ToolbarService
    {
        private readonly ToolbarView _view;

        public ToolbarService(ToolbarView view)
        {
            _view = Object.Instantiate(view);
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