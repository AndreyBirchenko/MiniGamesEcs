using Client.Cofigs;
using Client.Systems;
using Client.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace Client
{
    sealed class MenuEcsStartup : MonoBehaviour
    {
        [SerializeField] private MenuView _menuView;
        [SerializeField] private MenuCatalogConfig _menuCatalogConfig;

        EcsSystems _systems;

        void Start()
        {
            _systems = new EcsSystems(new EcsWorld());
            _systems
                .Add(new LoadMiniGameSystem())
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem())
#endif
                .Inject
                (
                    _menuView,
                    _menuCatalogConfig
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