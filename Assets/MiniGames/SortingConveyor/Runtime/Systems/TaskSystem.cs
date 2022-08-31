using Core.Components.Events;
using Core.Configs;
using Core.Services;
using Core.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.EndGame.Runtime;
using MiniGames.Core.Toolbar.Runtime;

using Utility;

using Extensions = Utility.Extensions;
using TaskService = Core.Services.TaskService;

namespace Core.Systems
{
    public class TaskSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AnswerPanelView _answerPanelView;
        private readonly EcsCustomInject<SortingConveyorConfig> _config = default;
        private int _correctAnswers;
        private EcsWorld _globalworld;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<Services.TaskService> _taskService = default;
        private readonly EcsFilterInject<Inc<GetTaskEvent>> f_getTask = Constants.Events;

        public void Init(IEcsSystems systems)
        {
            _globalworld = Extensions.GetGlobalWorld();

            _answerPanelView = _sceneData.Value.AnswerPanelView;
            _globalworld.SendShowToolbarEvent
                (_config.Value.IterationsCount, "Match the shapes by silhouette");

            GenerateTask();
        }

        public void Run(IEcsSystems systems)
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