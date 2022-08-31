using Core.Services.Toolbar.Configs;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using PoppingItems.Components;

using UnityEngine;

namespace PoppingItems.Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private EcsFilterInject<Inc<BubbleComponent>> f_bubble = default;
        private EcsCustomInject<PoppingItemsConfig> _config = default;
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in f_bubble.Value)
            {
                var pool = f_bubble.Pools.Inc1;
                ref var component = ref pool.Get(entity);
                var view = component.BubbleView;
                
                var velocity = Vector3.up * Time.deltaTime * _config.Value.BubbleSpeed;
                view.transform.Translate(velocity);
            }
        }
    }
}