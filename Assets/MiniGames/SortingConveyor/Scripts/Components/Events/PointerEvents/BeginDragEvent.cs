using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames.SortingConveyor.Components.Events
{
    public struct BeginDragEvent<T> where T : MonoBehaviour
    {
        public PointerEventData PointerEventData;
        public T View;
    }
}