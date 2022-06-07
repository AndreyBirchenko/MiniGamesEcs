using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Services.Toolbar.Components;
using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar;
using Core.Services.Toolbar.Views;

namespace Core.Systems
{
    public class HandleClickSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<ClickEvent<BubbleView>>> _clickFilter = Constants.Events;
        private readonly EcsWorldInject _eventWorld = Constants.Events;
        private readonly EcsCustomInject<ToolbarService> _toolbarService = default;
        private readonly EcsCustomInject<TaskService> _taskService = default;
        private readonly EcsCustomInject<PoppingItemsConfig> _config;

        private int _rightAnswersCounter;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _clickFilter.Value)
            {
                var pool = _clickFilter.Pools.Inc1;
                ref var clickEvent = ref pool.Get(entity);
                var bubbleView = clickEvent.View;
                pool.Del(entity);

                bubbleView.PlayPopAnimation();

                if (!bubbleView.PackedEntityWithWorld.Unpack(out var world, out var bubbleEntity)) continue;

                var taskPool = world.GetPool<TaskComponent>();
                ref var taskComponent = ref taskPool.Get(bubbleEntity);

                if (_taskService.Value.CheckAnswer(taskComponent))
                {
                    _toolbarService.Value.Fill();
                    _rightAnswersCounter++;
                }

                if (_rightAnswersCounter >= _config.Value.RightAnswersCount)
                {
                    var e = _eventWorld.Value.NewEntity();
                    var endGamePool = _eventWorld.Value.GetPool<EndGameEvent>();
                    endGamePool.Add(e);
                }
            }
        }
    }
}