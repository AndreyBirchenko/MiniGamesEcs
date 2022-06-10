using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar;
using Core.Systems;
using Core.Services.Toolbar.Views;

using UnityEngine;

using Utility;

namespace MiniGames.Poppingitems
{
    [AddComponentMenu(nameof(EcsStartup) + " in Popping Items")]
    sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private PoppingItemsConfig _config;
        [SerializeField] private Camera _camera;

        private EcsSystems _systems;
        private readonly MonoPool<BubbleView, BubbleView> _objectPool = new MonoPool<BubbleView, BubbleView>();
        private readonly TaskService _taskService = new TaskService();

        void Start()
        {
            _systems = new EcsSystems(new EcsWorld());
            _systems
                .Add(new StartupSystem())
                .Add(new TimerSystem<SpawnEvent>())
                .Add(new SpawnSystem(transform))
                .Add(new HandleClickSystem())
                .Add(new DestroySystem())
                .AddWorld(new EcsWorld(), Constants.Events)
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem(Constants.Events))
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject(
                    _config, 
                    _objectPool,
                    _camera,
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