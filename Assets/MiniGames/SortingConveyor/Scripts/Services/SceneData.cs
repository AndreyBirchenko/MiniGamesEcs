using Core.Views;

using UnityEngine;

namespace Core.Services
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Transform SpawnPoint { get; private set; }
        [field: SerializeField] public AnswerPanelView AnswerPanelView { get; private set; }
    }
}