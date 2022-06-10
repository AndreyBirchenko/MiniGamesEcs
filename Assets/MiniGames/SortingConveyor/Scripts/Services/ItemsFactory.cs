using Leopotam.EcsLite;

using MiniGames.SortingConveyor.Components;
using MiniGames.SortingConveyor.Views;

using UnityEngine;

using Utility;

namespace MiniGames.SortingConveyor.Services
{
    public class ItemsFactory
    {
        private readonly EcsWorld _world;
        private readonly EcsWorld _eventsWorld;
        private readonly ItemView[] _prefabs;
        private readonly MonoPool<ItemView, ItemView> _pool;
        private readonly GameObject _rootObject;

        private int _index = int.MaxValue;

        public ItemsFactory(
            EcsWorld world, 
            ItemView[] prefabs, 
            EcsWorld eventsWorld,
            Transform startupTransform
            )
        {
            _world = world;
            _prefabs = prefabs;
            _pool = new MonoPool<ItemView, ItemView>();
            _eventsWorld = eventsWorld;
            _rootObject = new GameObject("SpawnRoot");
            _rootObject.transform.SetParent(startupTransform);
        }

        public ItemView Get()
        {
            var itemView = _pool.Get(GetRandomPrefab());
            itemView.transform.SetParent(_rootObject.transform);

            var entity = _world.NewEntity();

            var itemsPool = _world.GetPool<ItemComponent>();
            ref var itemComponent = ref itemsPool.Add(entity);
            itemComponent.View = itemView;

            var horizontalPool = _world.GetPool<HorizontalMovementComponent>();
            ref var movementComponent = ref horizontalPool.Add(entity);
            movementComponent.Transform = itemView.transform;

            var packedEntity = _world.PackEntityWithWorld(entity);
            itemView.Construct(packedEntity, _eventsWorld);

            return itemView;
        }

        public void ReturnToPool(ItemView itemView)
        {
            if (itemView.PackedEntityWithWorld.Unpack(out var world, out var entity))
            {
                world.DelEntity(entity);
            }

            _pool.Return(itemView);
        }

        private ItemView GetRandomPrefab()
        {
            if (_index >= _prefabs.Length)
            {
                _index = 0;
                _prefabs.Shuffle();
            }

            return _prefabs[_index++];
        }
    }
}