using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Configs;
using MiniGames.SortingConveyor.Services;

using PoppingItems.Services;

using UnityEngine;

namespace MiniGames.SortingConveyor.Systems
{
    public class SpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsCustomInject<SortingConveyorConfig> _config = default;
        private readonly EcsCustomInject<ItemsFactory> _itemsFactory = default;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsFilterInject<Inc<SpawnEvent>> _spawnFilter = Constants.Events;
        private Vector3 _spawnPosition;
        private EcsWorldInject _world = default;

        public void Init(EcsSystems systems)
        {
            _spawnPosition = GetSpawnPosition();
        }

        public void Run(EcsSystems systems)
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