using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using PoppingItems.Components;

using UnityEngine;

using Utility;

namespace PoppingItems.Systems
{
    public class DestroySystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsCustomInject<Camera> _camera = default;
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;
        private readonly EcsFilterInject<Inc<DestroyEvent>> _destroyFilter = Constants.Events;
        private readonly EcsFilterInject<Inc<BubbleComponent>> _filter = default;
        private readonly EcsCustomInject<MonoPool<BubbleView, BubbleView>> _objectPool = default;
        private Vector3 _screenCentre;

        public void Init(IEcsSystems systems)
        {
            _screenCentre =
                _camera.Value.ScreenToWorldPoint(new Vector3((float) Screen.width / 2, (float) Screen.height / 2), 0);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter.Value)
            {
                var bubblePool = _filter.Pools.Inc1;
                ref var bubbleComponent = ref bubblePool.Get(entity);
                var bubbleView = bubbleComponent.BubbleView;

                var deltaY = bubbleView.transform.position.y - _screenCentre.y;
                if (!(deltaY >= _config.Value.DestroyOffset))
                    continue;
                bubblePool.Del(entity);
                _objectPool.Value.Return(bubbleView);
            }

            foreach (var entity in _destroyFilter.Value)
            {
                var pool = _destroyFilter.Pools.Inc1;
                ref var destroyEvent = ref pool.Get(entity);

                _objectPool.Value.Return(destroyEvent.BubbleView);
                pool.Del(entity);
            }
        }
    }
}