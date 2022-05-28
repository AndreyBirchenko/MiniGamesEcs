using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Poppingitems.Components;
using Poppingitems.Components.Events;
using Poppingitems.Configs;
using Poppingitems.Services;
using Poppingitems.Views;

using Utility;

using UnityEngine;

namespace Poppingitems.Systems
{
    sealed class SpawnSystem : IEcsRunSystem, IEcsInitSystem
    {
        private readonly EcsCustomInject<PoppingItemsConfig> _config = default;
        private readonly EcsCustomInject<MonoPool<BubbleView, BubbleView>> _objectPool = default;
        private readonly EcsCustomInject<TaskService> _taskService = default;

        private readonly EcsWorldInject _defaultWorld = default;
        private readonly EcsWorldInject _eventWorld = Constants.Events;

        private readonly EcsFilterInject<Inc<SpawnEvent>> _spawnFilter = Constants.Events;
        private readonly Transform _rootTransform;

        public SpawnSystem(Transform rootTransform)
        {
            _rootTransform = rootTransform;
        }

        private Vector3 _previousPosition;

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

                var randomTask = _taskService.Value.GetRandomTask();
                
                var bubbleEntity = _defaultWorld.Value.NewEntity();

                bubbleView.Initialize(randomTask.Answer.ToString());
                bubbleView.transform.position = GetUniquePosition();
                bubbleView.Rigidbody.velocity = _config.Value.BubbleVelocity;
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

                if (_previousPosition.Equals(position))
                {
                    continue;
                }

                _previousPosition = position;
                return position;
            }
        }
    }
}