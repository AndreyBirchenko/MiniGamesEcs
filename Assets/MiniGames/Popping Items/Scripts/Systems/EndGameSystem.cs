using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Core.Services.Toolbar.Components.Events;
using Core.Services.Toolbar;

namespace Core.Systems
{
    public class EndGameSystem : IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<EndGameEvent>> _endGameFilter = Constants.Events;
        private readonly EcsCustomInject<EndGameService> _endGameService = default;

        public void Run(EcsSystems systems)
        {
            foreach (var entity in _endGameFilter.Value)
            {
                _endGameService.Value.ShowEndGamePopup();

                var pool = _endGameFilter.Pools.Inc1;
                pool.Del(entity);
            }
        }
    }
}