using Core.Services.Toolbar;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.Toolbar.Runtime.Components;

namespace MiniGames.Core.Toolbar.Runtime
{
    public class ToolbarSystem : IEcsRunSystem
    {
        private EcsCustomInject<ToolbarService> _toolbarService = default;
        private EcsFilterInject<Inc<FillToolbarEvent>> f_fillEvent = default;
        private EcsFilterInject<Inc<ShowToolbarEvent>> f_showEvent = default;

        public void Run(IEcsSystems systems)
        {
            HandleFillEvent();
            HandleShowEvent();
        }

        private void HandleFillEvent()
        {
            foreach (var entity in f_fillEvent.Value)
            {
                _toolbarService.Value.Fill();
                f_fillEvent.Pools.Inc1.Del(entity);
            }
        }

        private void HandleShowEvent()
        {
            foreach (var entity in f_showEvent.Value)
            {
                ref var component = ref f_showEvent.Pools.Inc1.Get(entity);
                _toolbarService.Value.Show(component.MaxStepsCount, component.TaskText);
                f_showEvent.Pools.Inc1.Del(entity);
            }
        }
    }
}