using Leopotam.EcsLite;

using UnityEngine;

namespace Client
{
    public class GlobalWorldProvider : MonoBehaviour
    {
        private EcsWorld _globalWorld;
        
        public EcsWorld GetWorld()
        {
            return _globalWorld;
        }

        public void SetWorld(EcsWorld world)
        {
            _globalWorld = world;
        }
    }
}