using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Components.Events
{
    public struct DragEvent<T> where T : MonoBehaviour
    {
        public PointerEventData PointerEventData;
        public T View;
    }
}