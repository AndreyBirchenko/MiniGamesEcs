using Leopotam.EcsLite;

namespace MiniGames.Core.EndGame.Runtime
{
    public static class EndGameExtensions
    {
        public static void SendShowEndGamePopupEvent(this EcsWorld world)
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<ShowEndGamePopupEvent>();
            ref var component = ref pool.Add(entity);
        }
    }
}