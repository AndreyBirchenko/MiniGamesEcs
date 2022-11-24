using Core.Components;
using Core.Configs;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using UnityEngine;

namespace SortingConveyor.Systems
{
    public class MovementSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilterInject<Inc<HorizontalMovementComponent>> _horizontalFilter = default;
        private EcsFilterInject<Inc<VerticalMovementComponent>> _verticalFilter = default;
        private EcsCustomInject<SortingConveyorConfig> _config = default;

        private EcsPool<HorizontalMovementComponent> _horizontalPool;

        public void Init(IEcsSystems systems)
        {
            _horizontalPool = _horizontalFilter.Pools.Inc1;
        }

        public void Run(IEcsSystems systems)
        {
            HandleHorizontalMovement();
            HandleVerticalMovement();
        }

        private void HandleHorizontalMovement()
        {
            foreach (var entity in _horizontalFilter.Value)
            {
                ref var c_horizontalMovement = ref _horizontalPool.Get(entity);
                var transform = c_horizontalMovement.Transform;

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