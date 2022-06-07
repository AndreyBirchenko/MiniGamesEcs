using Core.Systems;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Configs;
using MiniGames.SortingConveyor.Systems;

using Core.Services.Toolbar;

using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Services;

using UnityEngine;

using DestroySystem = MiniGames.SortingConveyor.Systems.DestroySystem;
using SpawnSystem = MiniGames.SortingConveyor.Systems.SpawnSystem;
using TaskService = MiniGames.SortingConveyor.Services.TaskService;

namespace MiniGames.SortingConveyor
{
    [AddComponentMenu(nameof(EcsStartup) + " in Sorting Conveyor")]
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private SortingConveyorConfig _config;
        [SerializeField] private SceneData _sceneData;

        private EcsSystems _systems;
        private EcsWorld _world;
        private EcsWorld _eventsWorld;
        private ItemsFactory _itemsFactory;
        private TaskService _taskService;

        void Start()
        {
            Application.targetFrameRate = 60;

            _world = new EcsWorld();
            _eventsWorld = new EcsWorld();
            _itemsFactory = new ItemsFactory(_world, _config.Items, _eventsWorld);
            _taskService = new TaskService();

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
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Constants.Events))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(
                    _config,
                    _camera,
                    _sceneData,
                    _itemsFactory,
                    _taskService
                )
                .Init();
        }

        void Update()
        {
            _systems?.Run();
        }

        void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems.GetWorld(Constants.Events).Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}