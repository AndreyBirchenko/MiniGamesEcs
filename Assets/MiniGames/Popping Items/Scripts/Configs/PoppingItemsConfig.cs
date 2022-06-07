using Core.Services.Toolbar.Views;

using UnityEngine;

namespace Core.Services.Toolbar.Configs
{
    [CreateAssetMenu(menuName = "MiniGames/PoppingItemsConfig", order = 0)]
    public class PoppingItemsConfig : ScriptableObject
    {
        [field: SerializeField] public int RightAnswersCount { get; private set; }
        [field: SerializeField] public Vector2 BubbleVelocity { get; private set; }
        [field: SerializeField] public float DestroyOffset { get; private set; }
        [field: SerializeField] public BubbleView BubbleView { get; private set; }
        [field: SerializeField] public ToolbarView ToolbarView { get; private set; }
        [field: SerializeField] public EndGameView EndGameView { get; private set; }
        [field: SerializeField] public Vector3[] SpawnPoints { get; private set; }
    }
}