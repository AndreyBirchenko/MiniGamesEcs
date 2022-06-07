using System;

using MiniGames.SortingConveyor.Services;

using UnityEngine;

namespace MiniGames.SortingConveyor.Views
{
    public class AnswerView : MonoBehaviour
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [SerializeField] private Vector3 _initScale;

        public Vector3 InitScale => _initScale;

        private void OnValidate()
        {
            _initScale = transform.localScale;
        }
    }
}