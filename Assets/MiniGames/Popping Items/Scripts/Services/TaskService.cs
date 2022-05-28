using Poppingitems.Components;

using UnityEngine;

namespace Poppingitems.Services
{
    public class TaskService
    {
        public TaskComponent GlobalTask { get; private set; }
        private int _rightTaskSpawnChance = 50;

        public void GenerateGlobalTask()
        {
            var answer = Random.Range(0, 100);
            GlobalTask = new TaskComponent {Answer = answer};
        }

        public bool CheckAnswer(TaskComponent taskComponent)
        {
            return GlobalTask.Answer.Equals(taskComponent.Answer);
        }

        public TaskComponent GetRandomTask()
        {
            return Random.Range(0, 100) <= _rightTaskSpawnChance
                ? new TaskComponent {Answer = GlobalTask.Answer}
                : new TaskComponent {Answer = Random.Range(0, 100)};
        }
    }
}