using System.Threading;

using Core.Components.Events;

using Cysharp.Threading.Tasks;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.Core.Toolbar.Runtime;
using MiniGames.PointsDrawing.Components;
using MiniGames.PointsDrawing.Configs;
using MiniGames.PointsDrawing.Services;
using MiniGames.PointsDrawing.Views;

using UnityEngine;

using Utility;

using Extensions = Utility.Extensions;

namespace MiniGames.PointsDrawing.Systems
{
    public class LineDrawingSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsWorldInject _eventsWorld = Constants.Events;

        private EcsFilterInject<Inc<BeginDragEvent<DotView>>> f_beginDrag = Constants.Events;
        private EcsFilterInject<Inc<EndDragEvent<DotView>>> f_endDrag = Constants.Events;
        private EcsFilterInject<Inc<DotTriggerEnterEvent>> f_dotTriggered = Constants.Events;
        private EcsFilterInject<Inc<PointerClickedEvent<DotView>>> f_pointerClicked = Constants.Events;
        private EcsFilterInject<Inc<IterationStartEvent>> f_iterationStart = Constants.Events;

        private EcsCustomInject<PointsDrawingConfig> _config = default;
        private EcsCustomInject<Camera> _camera = default;
        private EcsCustomInject<IterationRepository> _repository = default;
        private EcsCustomInject<Transform> _rootTransform = default;

        private EcsWorld _globalWorld;
        private LineView _lineView;
        private DotView _lastConnectedDot;
        private CancellationTokenSource _destroyCts = new CancellationTokenSource();
        private bool _canDraw;
        private int _connectedDotsCounter;

        public void Init(IEcsSystems systems)
        {
            _globalWorld = Extensions.GetGlobalWorld();
        }
        
        public void Run(IEcsSystems systems)
        {
            HandleIterationStart();
            HandleDotClicked();
            HandleBeginDrag();
            HandleEndDrag();
            Draw();
            HandleAllDotsConnected();
        }

        public void Destroy(IEcsSystems systems)
        {
            _destroyCts.Cancel();
            _destroyCts.Dispose();
        }

        private void HandleIterationStart()
        {
            foreach (var entity in f_iterationStart.Value)
            {
                f_iterationStart.Pools.Inc1.Del(entity);
                _lineView = Object.Instantiate
                    (_config.Value.LineView, _rootTransform.Value);

                _lastConnectedDot = _repository.Value.GetStartPoint();
                var startPointPosition = _lastConnectedDot.transform.position;
                _lineView.SetFirstPoint(startPointPosition);
                _lineView.SetLastPoint(startPointPosition);
                _lineView.SetActive(false);
            }
        }

        private void HandleBeginDrag()
        {
            foreach (var entity in f_beginDrag.Value)
            {
                var pool = f_beginDrag.Pools.Inc1;
                ref var component = ref pool.Get(entity);
                var dot = component.View;

                if (dot.IsCorrectForStart)
                {
                    _canDraw = true;
                    _lastConnectedDot = dot;
                    _lineView.SetActive(true);
                }

                pool.Del(entity);
            }
        }

        private void HandleEndDrag()
        {
            foreach (var entity in f_endDrag.Value)
            {
                var pool = f_endDrag.Pools.Inc1;
                EndDraw();
                pool.Del(entity);
            }
        }

        private void EndDraw()
        {
            _canDraw = false;
            _lineView.SetToLastPoint(() => { _lineView.SetActive(false); });
        }

        private void HandleDotClicked()
        {
            foreach (var entity in f_pointerClicked.Value)
            {
                var pool = f_pointerClicked.Pools.Inc1;
                ref var component = ref pool.Get(entity);

                var clickedDot = component.View;

                if (DotIsConnectable(clickedDot))
                {
                    _lineView.SetActive(true);
                    _lineView.SetLastPoint(clickedDot.transform.position);
                    ConnectDot(clickedDot);
                }

                pool.Del(entity);
            }
        }

        private void Draw()
        {
            if (_canDraw == false)
                return;

            Vector2 pointerPosition = _camera.Value.ScreenToWorldPoint(Input.mousePosition);

            _lineView.SetLastPoint(pointerPosition);

            foreach (var entity in f_dotTriggered.Value)
            {
                var pool = f_dotTriggered.Pools.Inc1;
                ref var component = ref pool.Get(entity);

                var triggerredDot = component.DotView;

                if (DotIsConnectable(triggerredDot))
                {
                    ConnectDot(triggerredDot);
                }
                else if (triggerredDot.IsCorrectForStart == false)
                {
                    EndDraw();
                }

                pool.Del(entity);
            }
        }

        private bool DotIsConnectable(DotView dotView)
        {
            var dotIndex = GetDotIndex(dotView);
            var lastConnectedDotIndex = GetDotIndex(_lastConnectedDot);
            var firstDot = _repository.Value.DotViews[0];
            var beforeLastDotIndex =
                GetDotIndex(_repository.Value.DotViews[_repository.Value.DotViews.Count - 1]);

            if (dotView == firstDot && _connectedDotsCounter == 0) // first dot
                return false;
            if (dotIndex - lastConnectedDotIndex == 1) // next dot
                return true;
            return dotView == firstDot && lastConnectedDotIndex == beforeLastDotIndex; // last dot
        }

        private void ConnectDot(DotView dotView)
        {
            _lineView.AddPoint(dotView.transform.position);
            _lastConnectedDot.IsCorrectForStart = false;
            _lastConnectedDot = dotView;
            _lastConnectedDot.IsCorrectForStart = true;
            _connectedDotsCounter++;
            _globalWorld.SendFillToolbarEvent();
        }

        private void HandleAllDotsConnected()
        {
            var allDotsConnected = _connectedDotsCounter >= _repository.Value.DotViews.Count;
            if (allDotsConnected == false) return;
            _canDraw = false;
            _lineView.SetLastPoint(_repository.Value.DotViews[0].transform.position);
            _lineView.Hide();
            _repository.Value.DotViews.ForEach(x =>
            {
                x.SetActiveCollider(false);
                x.HideAsync(_destroyCts.Token).Forget();
            });
            _connectedDotsCounter = 0;
            _eventsWorld.Value.SendEmptyEvent<AllDotsConnectedEvent>();
        }

        private int GetDotIndex(DotView dotView)
        {
            return _repository.Value.DotViews.IndexOf(dotView);
        }
    }
}