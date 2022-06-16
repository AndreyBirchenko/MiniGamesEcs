using System;

using Random = UnityEngine.Random;

namespace Core.Services
{
    public class TaskService
    {
        private ItemType _currentTask;

        public ItemType GetTask()
        {
            var values = Enum.GetValues(typeof(ItemType));

            var task = ItemType.None;

            while (task.Equals(ItemType.None) || task.Equals(_currentTask))
            {
                task = (ItemType) values.GetValue(Random.Range(0, values.Length));
            }

            _currentTask = task;

            return _currentTask;
        }

        public bool AnswerIsCorrect(ItemType answerItemType)
        {
            return _currentTask.Equals(answerItemType);
        }
    }
}