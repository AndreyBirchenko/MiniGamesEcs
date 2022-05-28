using UnityEngine;
using UnityEngine.EventSystems;

namespace Poppingitems.Components.Events
{
    public struct ClickEvent<T> where T : MonoBehaviour
    {
        public PointerEventData PointerEventData;
        public T View;
    }
}