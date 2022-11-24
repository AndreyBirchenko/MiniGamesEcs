using DG.Tweening;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif

using MiniGames.PointsDrawing.Configs;
using MiniGames.PointsDrawing.Services;
using MiniGames.PointsDrawing.Systems;

using Utility;

using UnityEngine;

namespace MiniGames.PointsDrawing
{
    internal sealed class PointsDrawingEcsStartup : MonoBehaviour
    {
        [SerializeField] private PointsDrawingConfig _config;
        [SerializeField] private Camera _camera;


        private EcsSystems _systems;

        private void Start()
        {
            DOTween.SetTweensCapacity(200, 125);
            var iterationRepository = new IterationRepository();

            _systems = new EcsSystems(new EcsWorld());
            _systems
                .Add(new InitSystem())
                .Add(new IterationsControllerSystem())
                .Add(new LineDrawingSystem())
                .AddWorld(new EcsWorld(), Constants.Events)
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem(Constants.Events))
                .Add(new EcsWorldDebugSystem())
#endif
                .Inject(
                    _camera,
                    _config,
                    iterationRepository,
                    transform
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