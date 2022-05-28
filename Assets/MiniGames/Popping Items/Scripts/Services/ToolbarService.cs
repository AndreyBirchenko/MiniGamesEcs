using Poppingitems.Views;

using UnityEngine;

namespace Poppingitems.Services
{
    public class ToolbarService
    {
        private readonly ToolbarView _view;

        public ToolbarService(ToolbarView view)
        {
            _view = Object.Instantiate(view);
        }

        public void Show(int maxStepsCount, string taskText)
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