using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Services;
using MiniGames.SortingConveyor.Views;

using Core.Services.Toolbar;

using MiniGames.SortingConveyor.Configs;

using TaskService = MiniGames.SortingConveyor.Services.TaskService;

namespace MiniGames.SortingConveyor.Systems
{
    public class TaskSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<GetTaskEvent>> f_getTask = Constants.Events;
        private EcsCustomInject<TaskService> _taskService = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;
        private EcsCustomInject<EndGameService> _endGameService = default;
        private EcsCustomInject<ToolbarService> _toolbarService = default;

        private AnswerPanelView _answerPanelView;
        private int _correctAnswers;

        public void Init(EcsSystems systems)
        {
            _answerPanelView = _sceneData.Value.AnswerPanelView;
            _toolbarService.Value.Show(
                _config.Value.IterationsCount, "Match the shapes by silhouette");

            GenerateTask();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in f_getTask.Value)
            {
                if (++_correctAnswers >= _config.Value.IterationsCount)
                {
                    FillToolbar();
                    ShowEndGame();
                    f_getTask.Pools.Inc1.Del(entity);
                    return;
                }

                FillToolbar();
                GenerateTask();

                f_getTask.Pools.Inc1.Del(entity);
            }
        }

        private void GenerateTask()
        {
            _answerPanelView.SetAnswerView(_taskService.Value.GetTask());
        }

        private void ShowEndGame()
        {
            _endGameService.Value.ShowEndGamePopup();
        }

        private void FillToolbar()
        {
            _toolbarService.Value.Fill();
        }
    }
}