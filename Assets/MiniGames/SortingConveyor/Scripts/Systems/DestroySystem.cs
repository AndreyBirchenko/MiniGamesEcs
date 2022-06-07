using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components;
using MiniGames.SortingConveyor.Configs;
using MiniGames.SortingConveyor.Services;

using UnityEngine;

namespace MiniGames.SortingConveyor.Systems
{
    public class DestroySystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<ItemComponent>> _itemsFilter = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;
        private EcsCustomInject<ItemsFactory> _factory = default;

        public void Run(EcsSystems systems)
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