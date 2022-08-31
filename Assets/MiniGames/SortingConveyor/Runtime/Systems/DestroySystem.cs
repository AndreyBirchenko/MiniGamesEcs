using Core.Components;
using Core.Configs;
using Core.Services;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace SortingConveyor.Systems
{
    public class DestroySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<ItemComponent>> _itemsFilter = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;
        private EcsCustomInject<ItemsFactory> _factory = default;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _itemsFilter.Value)
            {
                var pool = _itemsFilter.Pools.Inc1;
                ref var itemComponent = ref pool.Get(entity);
                var itemView = itemComponent.View;

                if (itemView.IsDragging)
                    continue;

                var distance = Vector3.Distance(_sceneData.Value.SpawnPoint.position, itemView.transform.position);
                if (distance > _config.Value.DestroyDistance)
                {
                    _factory.Value.ReturnToPool(itemView);
                }
            }
        }
    }
}