using Core.Services.Toolbar;
using Core.Services.Toolbar.Configs;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.Toolbar.Runtime;

using PoppingItems.Services;

using Extensions = Utility.Extensions;

namespace PoppingItems.Systems
{
    public class StartupSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsCustomInject<ToolbarService> _toolbar = default;

        public void Init(EcsSystems systems)
        {
            var globalWorld = Extensions.GetGlobalWorld();

            _taskService.Value.GenerateGlobalTask();
            globalWorld.SendShowToolbarEvent(_config.Value.RightAnswersCount,
                $"Pop all bubbles with number {_taskService.Value.GlobalTask.Answer.ToString()}");
        }
    }
}