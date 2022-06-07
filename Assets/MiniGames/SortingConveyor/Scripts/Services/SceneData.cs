using MiniGames.SortingConveyor.Views;

using UnityEngine;

namespace MiniGames.SortingConveyor.Services
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Transform SpawnPoint { get; private set; }
        [field: SerializeField] public AnswerPanelView AnswerPanelView { get; private set; }
    }
}