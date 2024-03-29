﻿using Client.Cofigs;

using Core.Services;
using Core.Views;

using UnityEngine;

namespace Core.Configs
{
    [CreateAssetMenu(menuName = "MiniGames/SortingConveyorConfig", order = 0)]
    public class SortingConveyorConfig : BaseMiniGameConfig
    {
        [field: SerializeField] public int IterationsCount { get; private set; }
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public ItemView[] Items { get; private set; }
        [field: SerializeField] public float MoveSpeed { get; private set; }
        [field: SerializeField] public float FallSpeed { get; private set; }
        [field: SerializeField] public float DestroyDistance { get; private set; }
        
    }
}