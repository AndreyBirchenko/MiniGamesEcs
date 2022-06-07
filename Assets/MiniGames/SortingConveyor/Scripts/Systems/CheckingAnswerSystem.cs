using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

using MiniGames.SortingConveyor.Components;
using MiniGames.SortingConveyor.Components.Events;
using MiniGames.SortingConveyor.Services;
using MiniGames.SortingConveyor.Views;

using Poppingitems.Services;

using TaskService = MiniGames.SortingConveyor.Services.TaskService;

namespace MiniGames.SortingConveyor.Systems
{
    public class CheckingAnswerSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilterInject<Inc<CheckAnswerEvent>> f_checkAnswer = Constants.Events;
        private EcsPoolInject<CheckAnswerEvent> p_checkAnswer;
        private EcsPoolInject<VerticalMovementComponent> p_verticalMovement;
        private EcsCustomInject<SceneData> _sceneData = default;
        private EcsCustomInject<TaskService> _taskService = default;

        private AnswerPanelView _answerPanel;
        private EcsWorld _eventsWorld;

        public void Init(EcsSystems systems)
        {
            _answerPanel = _sceneData.Value.AnswerPanelView;
            _eventsWorld = systems.GetWorld(Constants.Events);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var entity in f_checkAnswer.Value)
            {
                var p_checkAnswer = f_checkAnswer.Pools.Inc1;
                ref var c_checkAnswer = ref p_checkAnswer.Get(entity);

                var itemView = c_checkAnswer.View;

                if (AnswerIsCorrect(itemView))
                {
                    itemView.PlayCorrectAnimation
                        (_answerPanel.transform.position, SendTaskSystemEvent);
                }
                else
                {
                    AddVerticalMovementComponent(itemView);
                }

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