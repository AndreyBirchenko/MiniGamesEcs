﻿using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Poppingitems.Components;
using Poppingitems.Components.Events;
using Poppingitems.Configs;
using Poppingitems.Services;
using Poppingitems.Views;

using UnityEngine;

using Utility;

namespace Poppingitems.Systems
{
    public class DestroySystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsFilterInject<Inc<BubbleComponent>> _filter = default;
        private readonly EcsFilterInject<Inc<DestroyEvent>> _destroyFilter = Constants.Events;
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;
        private readonly EcsCustomInject<MonoPool<BubbleView, BubbleView>> _objectPool = default;
        private readonly EcsCustomInject<Camera> _camera = default;
        private Vector3 _screenCentre;

        public void Init(EcsSystems systems)
        {
            _screenCentre =
                _camera.Value.ScreenToWorldPoint(new Vector3((float) Screen.width / 2, (float) Screen.height / 2), 0);
        }

        public void Run(EcsSystems systems)
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