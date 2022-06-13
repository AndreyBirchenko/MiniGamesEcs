using System.Collections.Generic;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components;
using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Views;

using PoppingItems.Services;

using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames.SortingConveyor.Systems
{
    public class DragSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsFilterInject<Inc<BeginDragEvent<ItemView>>> _beginDragFilter = Constants.Events;
        private readonly EcsCustomInject<Camera> _camera = default;
        private readonly EcsPoolInject<CheckAnswerEvent> _checkAnswerPool = Constants.Events;
        private readonly EcsFilterInject<Inc<DragEvent<ItemView>>> _dragFilter = Constants.Events;
        private readonly float _dragSpeed = 15f;
        private readonly EcsFilterInject<Inc<EndDragEvent<ItemView>>> _endDragFilter = Constants.Events;

        private EcsWorld _evensWorld;
        private Dictionary<PointerEventData, ItemView> _itemsInDrag;

        public void Init(EcsSystems systems)
        {
            _evensWorld = systems.GetWorld(Constants.Events);
            _itemsInDrag = new Dictionary<PointerEventData, ItemView>(4);
        }

        public void Run(EcsSystems systems)
        {
            HandleBeginDrag();
            HandleDrag();
            HandleEndDrag();
            DragObjects();
        }

        private void HandleBeginDrag()
        {
            foreach (var entity in _beginDragFilter.Value)
            {
                StopHorizontalMovement(entity);

                var p_beginDrag = _beginDragFilter.Pools.Inc1;
                ref var c_beginDrag = ref p_beginDrag.Get(entity);

                var itemView = c_beginDrag.View;
                var pointerData = c_beginDrag.PointerEventData;

                if (_itemsInDrag.ContainsKey(pointerData) == false) _itemsInDrag.Add(pointerData, itemView);

                p_beginDrag.Del(entity);
            }
        }

        private void HandleDrag()
        {
            foreach (var entity in _dragFilter.Value)
            {
                var pool = _dragFilter.Pools.Inc1;
                ref var component = ref pool.Get(entity);

                var itemView = component.View;
                var eventData = component.PointerEventData;

                _dragFilter.Pools.Inc1.Del(entity);
            }
        }

        private void HandleEndDrag()
        {
            foreach (var entity in _endDragFilter.Value)
            {
                var pool = _endDragFilter.Pools.Inc1;
                ref var component = ref pool.Get(entity);

                var pointerData = component.PointerEventData;
                var itemView = component.View;

                _itemsInDrag.Remove(pointerData);

                SendCheckAnswerEvent(itemView);

                _endDragFilter.Pools.Inc1.Del(entity);
            }
        }

        private void DragObjects()
        {
            foreach (var itemDataPair in _itemsInDrag)
            {
                var eventData = itemDataPair.Key;
                var itemView = itemDataPair.Value;
                var targetPosition = _camera.Value.ScreenToWorldPoint(eventData.position);
                itemView.transform.position = Vector2.Lerp(itemView.transform.position, targetPosition,
                    _dragSpeed * Time.deltaTime);
            }
        }

        private void StopHorizontalMovement(int entity)
        {
            var p_beginDrag = _beginDragFilter.Pools.Inc1;
            ref var c_beginDrag = ref p_beginDrag.Get(entity);

            var itemView = c_beginDrag.View;

            if (itemView.PackedEntityWithWorld.Unpack(out var world, out var itemEntity) == false)
                return;

            var p_items = world.GetPool<HorizontalMovementComponent>();
            if (p_items.Has(itemEntity))
                p_items.Del(itemEntity);
        }

        private void SendCheckAnswerEvent(ItemView itemView)
        {
            var entity = _evensWorld.NewEntity();
            ref var c_checkAnswer = ref _checkAnswerPool.Value.Add(entity);
            c_checkAnswer.View = itemView;
        }
    }
}