using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.EndGame.Runtime;
using MiniGames.Core.Toolbar.Runtime;
using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Configs;
using MiniGames.SortingConveyor.Services;
using MiniGames.SortingConveyor.Views;

using PoppingItems.Services;

using Extensions = Utility.Extensions;
using TaskService = MiniGames.SortingConveyor.Services.TaskService;

namespace MiniGames.SortingConveyor.Systems
{
    public class TaskSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AnswerPanelView _answerPanelView;
        private readonly EcsCustomInject<SortingConveyorConfig> _config = default;
        private int _correctAnswers;
        private EcsWorld _globalworld;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsFilterInject<Inc<GetTaskEvent>> f_getTask = Constants.Events;

        public void Init(EcsSystems systems)
        {
            _globalworld = Extensions.GetGlobalWorld();

            _answerPanelView = _sceneData.Value.AnswerPanelView;
            _globalworld.SendShowToolbarEvent
                (_config.Value.IterationsCount, "Match the shapes by silhouette");

            GenerateTask();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in f_getTask.Value)
            {
                if (++_correctAnswers >= _config.Value.IterationsCount)
                {
                    _globalworld.SendFillToolbarEvent();
                    _globalworld.SendShowEndGamePopupEvent();
                    f_getTask.Pools.Inc1.Del(entity);
                    return;
                }

                _globalworld.SendFillToolbarEvent();
                GenerateTask();

                f_getTask.Pools.Inc1.Del(entity);
            }
        }

        private void GenerateTask()
        {
            _answerPanelView.SetAnswerView(_taskService.Value.GetTask());
        }
    }
}