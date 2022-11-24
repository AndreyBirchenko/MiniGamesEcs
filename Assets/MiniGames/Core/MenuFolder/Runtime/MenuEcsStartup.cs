using Client.Cofigs;
using Client.Systems;
using Client.Views;

using Core.Services.Toolbar;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.EndGame.Runtime;
using MiniGames.Core.Toolbar.Runtime;

using UnityEngine;

namespace Client
{
    sealed class MenuEcsStartup : MonoBehaviour
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private MenuCatalogConfig _menuCatalogConfig;
        [SerializeField] private GlobalWorldProvider _globalWorldProvider;
        [SerializeField] private FadeScreenView _fadeScreenView;

        private EcsSystems _systems;

        void Start()
        {
            QualitySettings.vSyncCount = 2;
            
            var globalWorld = new EcsWorld();
            _globalWorldProvider.SetWorld(globalWorld);

            var toolbarService = new ToolbarService();
            var endGameService = new EndGameService();

            _systems = new EcsSystems(globalWorld);
            _systems
                .Add(new LoadMiniGameSystem())
                .Add(new ToolbarSystem())
                .Add(new EndGameSystem())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject
                (
                    _menuView,
                    _menuCatalogConfig,
                    toolbarService,
                    endGameService,
                    _fadeScreenView
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
                _systems.GetWorld().Destroy();
                _systems = null;
            }
        }
    }
}