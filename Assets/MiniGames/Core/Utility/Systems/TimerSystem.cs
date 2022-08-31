using Core.Interfaces;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Utility;

using UnityEngine;

namespace Core.Systems
{
    internal sealed class TimerSystem<T> : IEcsRunSystem, IEcsInitSystem
        where T : struct, ITimerComponent
    {
        private readonly EcsWorldInject _eventsworld = Constants.Events;
        private readonly EcsPoolInject<T> _pool = Constants.Events;
        private readonly ITimerComponent _timerComponent = new T();
        private float _time;

        public void Init(IEcsSystems systems)
        {
            _time = _timerComponent.Time;
        }

        public void Run(IEcsSystems systems)
        {
            _time -= Time.deltaTime;

            if (!(_time <= 0)) return;

            var entity = _eventsworld.Value.NewEntity();
            _pool.Value.Add(entity);

            _time = _timerComponent.Time;
        }
    }
}