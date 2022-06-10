using System;
using System.Collections.Generic;
using System.Linq;

using Client;

using Leopotam.EcsLite;

using Random = UnityEngine.Random;

namespace Utility
{
    public static class Extensions
    {
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Length);
                array[i] = array[random];
                array[random] = temp;
            }
        }

        public static void Shuffle<T>(this List<T> array)
        {
            for (int i = 0; i < array.Count; i++)
            {
                T temp = array[i];
                int random = Random.Range(i, array.Count);
                array[i] = array[random];
                array[random] = temp;
            }
        }

        public static T GetRandomElement<T>(this IEnumerable<T> source)
        {
            return source.Shuffle().First();
        }

        public static IEnumerable<T> GetRandomElements<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var i in source)
            {
                action.Invoke(i);
            }
        }

        public static EcsWorld GetGlobalWorld()
        {
            var worldProvider = UnityEngine.Object.FindObjectOfType<GlobalWorldProvider>();
            return worldProvider.GetWorld();
        }
    }
}