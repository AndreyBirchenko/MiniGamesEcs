using Core.Components;
using Core.Components.Events;
using Core.Services;
using Core.Views;

using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using Utility;

using TaskService = Core.Services.TaskService;

namespace Core.Systems
{
    public class CheckingAnswerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private AnswerPanelView _answerPanel;
        private EcsWorld _eventsWorld;
        private readonly EcsCustomInject<SceneData> _sceneData = default;
        private readonly EcsCustomInject<Services.TaskService> _taskService = default;
        private readonly EcsFilterInject<Inc<CheckAnswerEvent>> f_checkAnswer = Constants.Events;
        private EcsPoolInject<CheckAnswerEvent> p_checkAnswer;
        private EcsPoolInject<VerticalMovementComponent> p_verticalMovement;

        public void Init(IEcsSystems systems)
        {
            _answerPanel = _sceneData.Value.AnswerPanelView;
            _eventsWorld = systems.GetWorld(Constants.Events);
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in f_checkAnswer.Value)
            {
                var p_checkAnswer = f_checkAnswer.Pools.Inc1;
                ref var c_checkAnswer = ref p_checkAnswer.Get(entity);

                var itemView = c_checkAnswer.View;

                if (AnswerIsCorrect(itemView))
                    itemView.PlayCorrectAnimation
                        (_answerPanel.transform.position, SendTaskSystemEvent);
                else
                    AddVerticalMovementComponent(itemView);

                p_checkAnswer.Del(entity);
            }
        }

        private bool AnswerIsCorrect(ItemView answerItemView)
        {
            return _answerPanel.IsOverlap(answerItemView.transform.position) &&
                   _taskService.Value.AnswerIsCorrect(answerItemView.ItemType);
        }

        private void AddVerticalMovementComponent(ItemView itemView)
        {
            if (itemView.PackedEntityWithWorld.Unpack(out var world, out var entity) == false)
                return;

            if (p_verticalMovement.Value.Has(entity))
                return;

            ref var c_verticalMovement = ref p_verticalMovement.Value.Add(entity);
            c_verticalMovement.Transform = itemView.transform;
        }

        private void SendTaskSystemEvent()
        {
            var entity = _eventsWorld.NewEntity();
            var pool = _eventsWorld.GetPool<GetTaskEvent>();
            pool.Add(entity);
        }
    }
}