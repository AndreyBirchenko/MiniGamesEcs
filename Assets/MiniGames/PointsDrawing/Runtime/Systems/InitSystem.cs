using System.Linq;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.Toolbar.Runtime;
using MiniGames.PointsDrawing.Configs;
using MiniGames.PointsDrawing.Services;
using MiniGames.PointsDrawing.Views;

using Utility;

using Extensions = Utility.Extensions;

namespace MiniGames.PointsDrawing.Systems
{
    public class InitSystem : IEcsInitSystem
    {
        private EcsCustomInject<PointsDrawingConfig> _config = default;
        private readonly EcsCustomInject<IterationRepository> _repository = default;

        private EcsWorld _globalWorld;
        private UniqueRandomizer<ItemView> _itemRandomizer;


        public void Init(IEcsSystems systems)
        {
            _globalWorld = Extensions.GetGlobalWorld();
            _itemRandomizer = new UniqueRandomizer<ItemView>(_config.Value.ItemViews);

            GenerateItems();
            ShowToolbar();
        }

        private void GenerateItems()
        {
            foreach (var iteration in _config.Value.Iterations)
            {
                var itemPrefab = _itemRandomizer.Get();
                _repository.Value.ItemViews.Add(itemPrefab);
            }
        }

        private void ShowToolbar()
        {
            var answersCount = _repository.Value.ItemViews.Sum(view => view.Dots.Length);

            _globalWorld.SendShowToolbarEvent(answersCount, "Connect all the dots in order");
        }
    }
}