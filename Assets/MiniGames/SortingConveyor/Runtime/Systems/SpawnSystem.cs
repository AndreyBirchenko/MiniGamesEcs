using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Configs;
using Core.Components.Events;
using Core.Services;

using Utility;

using UnityEngine;

namespace SortingConveyor.Systems
{
    public class SpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsCustomInject<ItemsFactory> _itemsFactory = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsFilterInject<Inc<SpawnEvent>> _spawnFilter = Constants.Events;
        private Vector3 _spawnPosition;

        public void Init(IEcsSystems systems)
        {
            _spawnPosition = GetSpawnPosition();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _spawnFilter.Value)
            {
                var item = _itemsFactory.Value.Get();
                item.transform.position = _spawnPosition;

                _spawnFilter.Pools.Inc1.Del(entity);
            }
        }

        private Vector3 GetSpawnPosition()
        {
            return _sceneData.Value.SpawnPoint.position;
        }
    }
}