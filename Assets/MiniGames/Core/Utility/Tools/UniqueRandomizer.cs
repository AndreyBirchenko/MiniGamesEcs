using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public class UniqueRandomizer<T>
    {
        private List<T> _tempCollection;
        private List<T> _baseCollection;

        public UniqueRandomizer(params T[] type)
        {
            _baseCollection = new List<T>(type);
            _tempCollection = new List<T>(type);
            _tempCollection.Shuffle();
        }

        public T Get()
        {
            if (_tempCollection.Count == 0)
            {
                _tempCollection.AddRange(_baseCollection);
                _tempCollection.Shuffle();
            }

            var item = _tempCollection.First();
            _tempCollection.Remove(item);
            return item;
        }
    }
}