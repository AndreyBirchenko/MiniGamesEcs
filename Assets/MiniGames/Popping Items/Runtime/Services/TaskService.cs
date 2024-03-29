﻿using PoppingItems.Components;

using UnityEngine;

namespace Utility
{
    public class TaskService
    {
        private readonly int _rightTaskSpawnChance = 50;
        public TaskComponent GlobalTask { get; private set; }

        public void CreateGlobalTask()
        {
            var answer = Random.Range(0, 100);
            GlobalTask = new TaskComponent {Answer = answer};
        }

        public bool CheckAnswer(TaskComponent taskComponent)
        {
            return GlobalTask.Answer.Equals(taskComponent.Answer);
        }

        public TaskComponent CreateRandomTask()
        {
            return Random.Range(0, 100) <= _rightTaskSpawnChance
                ? new TaskComponent {Answer = GlobalTask.Answer}
                : new TaskComponent {Answer = Random.Range(0, 100)};
        }
    }
}