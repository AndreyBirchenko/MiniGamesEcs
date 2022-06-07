using Core.Interfaces;

namespace Poppingitems.Components.Events
{
    public struct SpawnEvent : ITimerComponent
    {
        public float Time => 0.5f;
    }
}