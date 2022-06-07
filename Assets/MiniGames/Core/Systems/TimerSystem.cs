using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Interfaces;
using Poppingitems.Services;

using UnityEngine;

namespace Core.Systems
{
    sealed class TimerSystem<T> : IEcsRunSystem, IEcsInitSystem 
        where T : struct, ITimerComponent
    {
        private readonly EcsWorldInject _eventsworld = Constants.Events;
        private readonly EcsPoolInject<T> _pool = Constants.Events;
        private readonly ITimerComponent _timerComponent = new T();
        private float _time;

        public void Init(EcsSystems systems)
        {
            _time = _timerComponent.Time;
        }

        public void Run(EcsSystems systems)
        {
            _time -= Time.deltaTime;

            if (!(_time <= 0)) return;
            
            var entity = _eventsworld.Value.NewEntity();
            _pool.Value.Add(entity);
                
            _time = _timerComponent.Time;
        }
    }
}