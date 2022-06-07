using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Services;
using MiniGames.SortingConveyor.Views;

using Poppingitems.Services;

using TaskService = MiniGames.SortingConveyor.Services.TaskService;

namespace MiniGames.SortingConveyor.Systems
{
    public class TaskSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<GetTaskEvent>> f_getTask = Constants.Events;
        private EcsCustomInject<TaskService> _taskService = default;
        private EcsCustomInject<SceneData> _sceneData = default;

        private AnswerPanelView _answerPanelView;

        public void Init(EcsSystems systems)
        {
            _answerPanelView = _sceneData.Value.AnswerPanelView;
            
            GenerateTask();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in f_getTask.Value)
            {
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