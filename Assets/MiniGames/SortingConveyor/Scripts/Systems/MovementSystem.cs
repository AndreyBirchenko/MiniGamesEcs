using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components;
using MiniGames.SortingConveyor.Configs;

using UnityEngine;

namespace MiniGames.SortingConveyor.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<HorizontalMovementComponent>> _horizontalFilter = default;
        private EcsFilterInject<Inc<VerticalMovementComponent>> _verticalFilter = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;

        public void Run(EcsSystems systems)
        {
            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void HandleHorizontalMovement()
        {
            foreach (var entity in _horizontalFilter.Value)
            {
                var pool = _horizontalFilter.Pools.Inc1;
                var transform = pool.Get(entity).Transform;

                var velocity = Vector3.right * Time.deltaTime * _config.Value.MoveSpeed;
                transform.Translate(velocity);
            }
        }

        private void HandleVerticalMovement()
        {
            foreach (var entity in _verticalFilter.Value)
            {
                var pool = _verticalFilter.Pools.Inc1;
                ref var c_verticalMovement = ref pool.Get(entity);
                var transform = c_verticalMovement.Transform;

                var velocity = Vector3.down * Time.deltaTime * _config.Value.FallSpeed;
                transform.Translate(velocity);
            }
        }
    }
}