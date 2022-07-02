using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Services.Toolbar.Components.Events
{
    public struct ClickEvent<T> where T : MonoBehaviour
    {
        public PointerEventData PointerEventData;
        public T View;
    }
}