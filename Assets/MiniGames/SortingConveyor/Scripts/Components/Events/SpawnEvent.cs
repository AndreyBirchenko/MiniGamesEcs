using Core.Interfaces;

namespace MiniGames.SortingConveyor.Components.Events
{
    public struct SpawnEvent : ITimerComponent
    {
        public float Time => 0.7f;
    }
}