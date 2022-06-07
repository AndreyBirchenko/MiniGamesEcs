using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Configs;
using MiniGames.SortingConveyor.Services;

using Poppingitems.Services;

using UnityEngine;

namespace MiniGames.SortingConveyor.Systems
{
    public class SpawnSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsCustomInject<ItemsFactory> _itemsFactory = default;
        private EcsFilterInject<Inc<SpawnEvent>> _spawnFilter = Constants.Events;
        private Vector3 _spawnPosition;
        
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