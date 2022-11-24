using System;

using Core.Components.Events;
using Core.Configs;
using Core.Services;
using Core.Systems;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

using Utility;

using SortingConveyor.Systems;

using UnityEngine;

using TaskService = Core.Services.TaskService;

namespace Core
{
    [AddComponentMenu(nameof(SortingConveyorEcsStartup) + " in Sorting Conveyor")]
    internal sealed class SortingConveyorEcsStartup : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SortingConveyorConfig _config;
        [SerializeField] private SceneData _sceneData;
        private EcsWorld _eventsWorld;

        private EcsSystems _systems;
        private EcsWorld _world;

        private void Start()
        {
            _world = new EcsWorld();
            _eventsWorld = new EcsWorld();
            var itemsFactory = new ItemsFactory(_world, _config.Items, _eventsWorld, transform);
            var taskService = new TaskService();

            _systems = new EcsSystems(_world);
            _systems
                .Add(new TaskSystem())
                .Add(new TimerSystem<SpawnEvent>())
                .Add(new SpawnSystem())
                .Add(new MovementSystem())
                .Add(new DragSystem())
                .Add(new CheckingAnswerSystem())
                .Add(new DestroySystem())
                .AddWorld(_eventsWorld, Constants.Events)
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem(Constants.Events))
                .Add(new EcsWorldDebugSystem())
#endif
                .Inject(
                    _config,
                    _camera,
                    _sceneData,
                    itemsFactory,
                    taskService
                )
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.GetWorld(Constants.Events).Destroy();
                _systems.GetWorld().Destroy();
                _systems.Destroy();
                _systems = null;
            }
        }
    }
}