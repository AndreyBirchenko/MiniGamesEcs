using System;
using System.Threading;

using Core.Components.Events;

using Cysharp.Threading.Tasks;

using DG.Tweening;

using Leopotam.EcsLite;

using MiniGames.PointsDrawing.Components;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames.PointsDrawing.Views
{
    public class DotView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler
    {
        [SerializeField] private TextMeshPro _textMeshPro;
        [SerializeField] private Collider2D _collider2D;

        private readonly float _animationDuration = 0.25f;

        private EcsWorld _eventsWorld;
        private Vector3 _initScale;
        private Sequence _sequence;

        public bool IsCorrectForStart { get; set; }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            var entity = _eventsWorld.NewEntity();
            var pool = _eventsWorld.GetPool<BeginDragEvent<DotView>>();
            ref var component = ref pool.Add(entity);
            component.PointerEventData = eventData;
            component.View = this;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            //do not delete this method
            //otherwise OnBeginDrag and OnEndDrag will not work
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            var entity = _eventsWorld.NewEntity();
            var pool = _eventsWorld.GetPool<EndDragEvent<DotView>>();
            ref var component = ref pool.Add(entity);
            component.PointerEventData = eventData;
            component.View = this;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            var entity = _eventsWorld.NewEntity();
            var pool = _eventsWorld.GetPool<PointerClickedEvent<DotView>>();
            ref var component = ref pool.Add(entity);
            component.PointerEventData = eventData;
            component.View = this;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var entity = _eventsWorld.NewEntity();
            var pool = _eventsWorld.GetPool<DotTriggerEnterEvent>();
            ref var component = ref pool.Add(entity);
            component.DotView = this;
        }

        public void Construct(EcsWorld eventsWorld)
        {
            _eventsWorld = eventsWorld;
            _initScale = transform.localScale;
            transform.localScale = Vector3.zero;
        }

        public void SetText(string text)
        {
            _textMeshPro.text = text;
        }

        public async UniTask ShowAsync(CancellationToken token)
        {
            _sequence = GetSequence();
            _sequence
                .Append(transform.DOScale(_initScale * 1.25f, _animationDuration / 2))
                .Append(transform.DOScale(_initScale, _animationDuration / 2));
            token.Register(() => GetSequence());
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration), cancellationToken: token);
        }

        public async UniTask HideAsync(CancellationToken token)
        {
            _sequence = GetSequence();
            _sequence
                .Append(transform.DOScale(_initScale * 1.25f, _animationDuration))
                .Append(transform.DOScale(Vector3.zero, _animationDuration));
            token.Register(() => GetSequence());
            await UniTask.Delay(TimeSpan.FromSeconds(_animationDuration), cancellationToken: token);
        }

        public void SetActiveCollider(bool condition)
        {
            _collider2D.enabled = condition;
        }

        private Sequence GetSequence()
        {
            if (_sequence.IsActive() && _sequence.IsPlaying())
            {
                _sequence.Kill();
                _sequence = null;
            }

            return DOTween.Sequence();
        }
    }
}