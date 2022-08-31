using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using PoppingItems.Components;
using PoppingItems.Services;

using UnityEngine;

using Utility;

namespace PoppingItems.Systems
{
    internal sealed class SpawnSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;

        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = Constants.Events;
        private readonly EcsCustomInject<MonoPool<BubbleView, BubbleView>> _objectPool = default;
        private readonly Transform _rootTransform;

        private readonly EcsFilterInject<Inc<SpawnEvent>> _spawnFilter = Constants.Events;
        private readonly EcsCustomInject<TaskService> _taskService = default;

        private Vector3 _previousPosition;

        public SpawnSystem(Transform rootTransform)
        {
            _rootTransform = rootTransform;
        }

        public void Init(EcsSystems systems)
        {
            _objectPool.Value.SetRootTransform(_rootTransform);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _spawnFilter.Value)
            {
                var prefab = _config.Value.BubbleView;
                var bubbleView = _objectPool.Value.Get(prefab);

                var randomTask = _taskService.Value.CreateRandomTask();

                var bubbleEntity = _defaultWorld.Value.NewEntity();

                bubbleView.Initialize(randomTask.Answer.ToString());
                bubbleView.transform.position = GetUniquePosition();
                bubbleView.PackedEntityWithWorld = _defaultWorld.Value.PackEntityWithWorld(bubbleEntity);

                var bubblesPool = _defaultWorld.Value.GetPool<BubbleComponent>();
                ref var bubbleComponent = ref bubblesPool.Add(bubbleEntity);
                bubbleComponent.BubbleView = bubbleView;
                bubbleComponent.BubbleView.EcsEventWorld = _eventWorld.Value;

                var tasksPool = _defaultWorld.Value.GetPool<TaskComponent>();
                ref var taskComponent = ref tasksPool.Add(bubbleEntity);
                taskComponent = randomTask;

                _spawnFilter.Pools.Inc1.Del(entity);
            }
        }

        private Vector3 GetUniquePosition()
        {
            while (true)
            {
                var position = _config.Value.SpawnPoints.GetRandomElement();

                if (_previousPosition.Equals(position)) continue;

                _previousPosition = position;
                return position;
            }
        }
    }
}