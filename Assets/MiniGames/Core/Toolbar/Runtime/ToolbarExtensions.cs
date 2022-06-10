using Leopotam.EcsLite;

using MiniGames.Core.Toolbar.Runtime.Components;

namespace MiniGames.Core.Toolbar.Runtime
{
    public static class ToolbarExtensions
    {
        public static void SendFillToolbarEvent(this EcsWorld world)
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<FillToolbarEvent>();
            ref var component = ref pool.Add(entity);
        }

        public static void SendShowToolbarEvent(this EcsWorld world, int maxStepsCount, string taskText)
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<ShowToolbarEvent>();
            ref var component = ref pool.Add(entity);
            component.MaxStepsCount = maxStepsCount;
            component.TaskText = taskText;
        }
    }
}