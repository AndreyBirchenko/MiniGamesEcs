using Client.Cofigs;

using Core.Services.Toolbar.Views;

using UnityEngine;

namespace Core.Services.Toolbar.Configs
{
    [CreateAssetMenu(menuName = "MiniGames/PoppingItemsConfig", order = 0)]
    public class PoppingItemsConfig : BaseMiniGameConfig
    {
        [field: SerializeField] public int RightAnswersCount { get; private set; }
        [field: SerializeField] public float BubbleSpeed { get; private set; }
        [field: SerializeField] public float DestroyOffset { get; private set; }
        [field: SerializeField] public BubbleView BubbleView { get; private set; }
        [field: SerializeField] public Vector3[] SpawnPoints { get; private set; }
        [field: SerializeField] public Color[] BubbleColors { get; private set; }
    }
}