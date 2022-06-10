using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar;

using MiniGames.Core.Toolbar.Runtime;

using Extensions = Utility.Extensions;

namespace Core.Systems
{
    public class StartupSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsCustomInject<ToolbarService> _toolbar = default;
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;

        public void Init(EcsSystems systems)
        {
            var globalWorld = Extensions.GetGlobalWorld();

            _taskService.Value.GenerateGlobalTask();
            globalWorld.SendShowToolbarEvent(_config.Value.RightAnswersCount,
                $"Pop all bubbles with number {_taskService.Value.GlobalTask.Answer.ToString()}");
        }
    }
}