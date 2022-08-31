using System;
using System.Reflection;

using Core.Components.Events;
using Core.Services;

using DG.Tweening;

using Leopotam.EcsLite;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace Core.Views
{
    public class ItemView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [SerializeField] private Collider2D _collider;
        [SerializeField] private SortingGroup _sortingGroup;

        public EcsPackedEntityWithWorld PackedEntityWithWorld { get; private set; }
        public EcsWorld EcsEventWorld { get; private set; }
        public bool IsDragging { get; private set; }

        private Sequence _sequence;
        private Vector3 _initScale;
        private int _initSorting;

        private void OnValidate()
        {
            if (_collider is null)
                _collider = GetComponent<Collider2D>();
        }

        private void Awake()
        {
            _initScale = transform.localScale;
            _initSorting = _sortingGroup.sortingOrder;
        }

        public void Construct(EcsPackedEntityWithWorld entityWithWorld, EcsWorld eventWorld)
        {
            PackedEntityWithWorld = entityWithWorld;
            EcsEventWorld = eventWorld;

            transform.localScale = _initScale;
            _collider.enabled = true;

            SetSorting(_initSorting);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            SetSorting(999);
            SetScale(_initScale * 1.25f);

            SendBeginDragEvent(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            SendDragEvent(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
            SetScale(_initScale);

            SendEndDragEvent(eventData);
        }

        public void PlayCorrectAnimation(Vector2 answerPanelPosition, Action call)
        {
            _sequence = GetSequence();
            _sequence
                .AppendCallback(() => _collider.enabled = false)
                .Append(transform.DOMove(answerPanelPosition, 0.5f))
                .Append(transform.DOScale(_initScale * 1.25f, 0.3f))
                .Append(transform.DOScale(0, 0.3f))
                .AppendCallback(call.Invoke);
        }

        private void SendBeginDragEvent(PointerEventData eventData)
        {
            var entity = EcsEventWorld.NewEntity();
            var pool = EcsEventWorld.GetPool<BeginDragEvent<ItemView>>();
            ref var eventComponent = ref pool.Add(entity);
            eventComponent.View = this;
            eventComponent.PointerEventData = eventData;
        }

        private void SendDragEvent(PointerEventData eventData)
        {
            var entity = EcsEventWorld.NewEntity();
            var pool = EcsEventWorld.GetPool<DragEvent<ItemView>>();
            ref var eventComponent = ref pool.Add(entity);
            eventComponent.View = this;
            eventComponent.PointerEventData = eventData;
        }

        private void SendEndDragEvent(PointerEventData eventData)
        {
            var entity = EcsEventWorld.NewEntity();
            var pool = EcsEventWorld.GetPool<EndDragEvent<ItemView>>();
            ref var eventComponent = ref pool.Add(entity);
            eventComponent.View = this;
            eventComponent.PointerEventData = eventData;
        }

        private void SetSorting(int value)
        {
            _sortingGroup.sortingOrder = value;
        }

        private void SetScale(Vector3 scale)
        {
            transform.DOScale(scale, 0.2f);
        }

        private Sequence GetSequence()
        {
            if (_sequence != null && _sequence.IsActive())
            {
                _sequence.Kill();
            }

            return DOTween.Sequence();
        }

        private void OnDestroy()
        {
            GetSequence();
        }
    }
}