using Leopotam.EcsLite;

namespace Utility
{
    public static class StaticActions
    {
        public static ref T CreateEventComponent<T>(EcsWorld world) where T : struct
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<T>();
            return ref pool.Add(entity);
        }
    }
}