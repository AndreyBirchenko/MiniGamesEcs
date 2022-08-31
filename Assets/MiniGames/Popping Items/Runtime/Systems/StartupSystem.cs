using Core.Services.Toolbar;
using Core.Services.Toolbar.Configs;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.Toolbar.Runtime;

using Utility;

using Extensions = Utility.Extensions;

namespace PoppingItems.Systems
{
    public class StartupSystem : IEcsInitSystem
    {
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;
        private readonly EcsCustomInject<TaskService> _taskService = default;

        public void Init(IEcsSystems systems)
        {
            var globalWorld = Extensions.GetGlobalWorld();

            _taskService.Value.CreateGlobalTask();
            globalWorld.SendShowToolbarEvent(_config.Value.RightAnswersCount,
                $"Pop all bubbles with number {_taskService.Value.GlobalTask.Answer.ToString()}");
        }
    }
}