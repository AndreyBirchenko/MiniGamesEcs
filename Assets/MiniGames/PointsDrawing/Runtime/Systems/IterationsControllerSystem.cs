using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.EndGame.Runtime;
using MiniGames.PointsDrawing.Components;
using MiniGames.PointsDrawing.Configs;
using MiniGames.PointsDrawing.Services;

using UnityEngine;

using Utility;

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Extensions = Utility.Extensions;

namespace MiniGames.PointsDrawing.Systems
{
    public class IterationsControllerSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorldInject _eventsWorld = Constants.Events;
        private readonly EcsCustomInject<PointsDrawingConfig> _config = default;
        private readonly EcsCustomInject<IterationRepository> _repository = default;
        private readonly EcsCustomInject<Transform> _rootTransform = default;
        private readonly EcsFilterInject<Inc<AllDotsConnectedEvent>> f_allDotsConnected = Constants.Events;

        private EcsWorld _globalWorld;
        private int _currentIterationIndex;
        private CancellationTokenSource _destroyCts;

        public void Init(IEcsSystems systems)
        {
            _destroyCts = new CancellationTokenSource();
            _globalWorld = Extensions.GetGlobalWorld();
            LoadIterationAsync(_destroyCts.Token).Forget();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in f_allDotsConnected.Value)
            {
                f_allDotsConnected.Pools.Inc1.Del(entity);
                ChangeIterationAsync(_destroyCts.Token).Forget();
            }
        }

        public void Destroy(IEcsSystems systems)
        {
            _destroyCts.Cancel();
            _destroyCts.Dispose();
        }

        private async UniTaskVoid ChangeIterationAsync(CancellationToken token)
        {
            await _repository.Value.CurrentItemView.ShowAsync(_destroyCts.Token);
            await UniTask.Delay(TimeSpan.FromSeconds(1.5f), cancellationToken: token);
            if (++_currentIterationIndex > _config.Value.Iterations.Length - 1)
            {
                _globalWorld.SendShowEndGamePopupEvent();
                return;
            }

            await LoadIterationAsync(token);
        }

        private async UniTask LoadIterationAsync(CancellationToken token)
        {
            await ClearIterationAsync(token);
            CreateItemView();
            CreateDots(_currentIterationIndex);
            _eventsWorld.Value.SendEmptyEvent<IterationStartEvent>();
            await ShowDotsAsync(token);
        }

        private async UniTask ClearIterationAsync(CancellationToken token)
        {
            var itemView = _repository.Value.CurrentItemView;
            if (itemView is null)
                return;
            await itemView.HideAsync(token);
        }

        private void CreateItemView()
        {
            var itemPrefab = _repository.Value.ItemViews[_currentIterationIndex];
            var itemView = Object.Instantiate(itemPrefab, _rootTransform.Value);
            itemView.Construct();
            _repository.Value.CurrentItemView = itemView;
        }

        private void CreateDots(int iterationIndex)
        {
            _repository.Value.DotViews.Clear();
            var startingNumber = _config.Value.Iterations[iterationIndex].StartingNumber;
            var itemView = _repository.Value.CurrentItemView;

            for (var i = 0; i < itemView.Dots.Length; i++)
            {
                var dot = itemView.Dots[i];
                var dotView = Object.Instantiate(_config.Value.DotView, dot);
                dotView.Construct(_eventsWorld.Value);
                dotView.SetText((startingNumber + i).ToString());

                _repository.Value.DotViews.Add(dotView);
            }
        }

        private async UniTask ShowDotsAsync(CancellationToken token)
        {
            foreach (var dotView in _repository.Value.DotViews)
                await dotView.ShowAsync(token);
        }
    }
}