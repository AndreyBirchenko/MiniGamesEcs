using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar;

namespace Core.Systems
{
    public class StartupSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsCustomInject<ToolbarService> _toolbar = default;
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;

        public void Init(EcsSystems systems)
        {
            _taskService.Value.GenerateGlobalTask();
            _toolbar.Value.Show(_config.Value.RightAnswersCount,
                $"Pop all bubbles with number {_taskService.Value.GlobalTask.Answer.ToString()}");
        }
    }
}