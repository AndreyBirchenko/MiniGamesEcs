using System.Collections.Generic;

using UnityEngine;

namespace Utility
{
    public class MonoPool<T, V> where T : Behaviour where V : Behaviour
    {
        private readonly Dictionary<string, Queue<T>> _behaviours;
        private readonly V _prefab;
        private Transform _rootTransform;

        public MonoPool()
        {
            _behaviours = new Dictionary<string, Queue<T>>();
        }

        public MonoPool(V prefab)
        {
            _behaviours = new Dictionary<string, Queue<T>>();
            _prefab = prefab;
        }

        public void SetRootTransform(Transform transform)
        {
            _rootTransform = transform;
        }

        public T Get()
        {
            return GetBehaviour(_prefab);
        }

        public T Get(V prefab)
        {
            return GetBehaviour(prefab);
        }

        private T GetBehaviour(V prefab)
        {
            if (_behaviours.TryGetValue(prefab.name, out var behaviours))
            {
                if (behaviours.Count == 0)
                    return Create(prefab);

                var behaviour = behaviours.Dequeue();
                behaviour.gameObject.SetActive(true);
                return behaviour;
            }

            return Create(prefab);
        }

        private T Create(V prefab)
        {
            var behaviour = _rootTransform ? 
                Object.Instantiate(prefab, _rootTransform) : 
                Object.Instantiate(prefab);

            behaviour.name = prefab.name;

            if (typeof(T) == typeof(V))
                return behaviour as T;

            return behaviour.GetComponent<T>();
        }

        public void Return(T behaviour, bool disableAfterReturning = true)
        {
            if (!behaviour)
            {
                return;
            }

            if (_behaviours.TryGetValue(behaviour.name, out var behaviours))
            {
                behaviours.Enqueue(behaviour);
            }
            else
            {
                var newQueue = new Queue<T>();
                newQueue.Enqueue(behaviour);
                _behaviours.Add(behaviour.name, newQueue);
            }

            if (disableAfterReturning)
                behaviour.gameObject.SetActive(false);
        }
    }
}