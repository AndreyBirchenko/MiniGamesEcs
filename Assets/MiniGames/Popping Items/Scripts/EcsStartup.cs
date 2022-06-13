using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar.Configs;
using Core.Services.Toolbar.Views;
using Core.Systems;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;

using PoppingItems.Services;
using PoppingItems.Systems;

using UnityEngine;

using Utility;

namespace MiniGames.Poppingitems
{
    [AddComponentMenu(nameof(EcsStartup) + " in Popping Items")]
    internal sealed class EcsStartup : MonoBehaviour
    {
        [SerializeField] private PoppingItemsConfig _config;
        [SerializeField] private Camera _camera;
        private readonly MonoPool<BubbleView, BubbleView> _objectPool = new MonoPool<BubbleView, BubbleView>();
        private readonly TaskService _taskService = new TaskService();

        private EcsSystems _systems;

        private void Start()
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
                .Add(new EcsWorldDebugSystem(Constants.Events))
                .Add(new EcsWorldDebugSystem())
#endif
                .Inject(
                    _config,
                    _objectPool,
                    _camera,
                    _taskService
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
                _systems.Destroy();
                _systems.GetWorld(Constants.Events).Destroy();
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}