using Core.Services.Toolbar;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace MiniGames.Core.EndGame.Runtime
{
    public class EndGameSystem : IEcsRunSystem
    {
        private EcsCustomInject<EndGameService> _endGameService = default;
        private EcsFilterInject<Inc<ShowEndGamePopupEvent>> f_endGamePopup = default;

        public void Run(EcsSystems systems)
        {
            HandleShowEndGamePopupEvent();
        }

        private void HandleShowEndGamePopupEvent()
        {
            foreach (var entity in f_endGamePopup.Value)
            {
                _endGameService.Value.ShowEndGamePopup();
                f_endGamePopup.Pools.Inc1.Del(entity);
            }
        }
    }
}