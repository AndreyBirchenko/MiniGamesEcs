using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Poppingitems.Configs;
using Poppingitems.Services;

namespace Poppingitems.Systems
{
    public class StartupSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsCustomInject<ToolbarService> _toolbar = default;
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;

        public void Init(EcsSystems systems)
        {
            _taskService.Value.GenerateGlobalTask();
            _toolbar.Value.Show(_config.Value.RightAnswersCount, _taskService.Value.GlobalTask.Answer.ToString());
        }
    }
}