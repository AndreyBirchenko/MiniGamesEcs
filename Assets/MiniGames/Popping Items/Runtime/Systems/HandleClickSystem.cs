using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.EndGame.Runtime;
using MiniGames.Core.Toolbar.Runtime;

using PoppingItems.Components;
using PoppingItems.Services;

using Extensions = Utility.Extensions;

namespace PoppingItems.Systems
{
    public class HandleClickSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<ClickEvent<BubbleView>>> _clickFilter = Constants.Events;
        private readonly EcsCustomInject<PoppingItemsConfig> _config;
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private EcsWorld _globalWorld;

        private int _rightAnswersCounter;

        public void Init(EcsSystems systems)
        {
            _globalWorld = Extensions.GetGlobalWorld();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _clickFilter.Value)
            {
                var pool = _clickFilter.Pools.Inc1;
                ref var clickEvent = ref pool.Get(entity);
                var bubbleView = clickEvent.View;
                pool.Del(entity);

                bubbleView.PlayPopAnimation();

                if (!bubbleView.PackedEntityWithWorld.Unpack(out var world, out var bubbleEntity)) 
                    continue;

                var taskPool = world.GetPool<TaskComponent>();
                ref var taskComponent = ref taskPool.Get(bubbleEntity);

                if (_taskService.Value.CheckAnswer(taskComponent))
                {
                    _globalWorld.SendFillToolbarEvent();
                    _rightAnswersCounter++;
                }

                if (_rightAnswersCounter >= _config.Value.RightAnswersCount) 
                    _globalWorld.SendShowEndGamePopupEvent();
            }
        }
    }
}