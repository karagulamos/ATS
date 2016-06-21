using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Library.Services.Common.DataStructures
{
    public sealed class ConcurrentHashSet<T> : ICollection<T>
    {
        public ConcurrentHashSet() : this(EqualityComparer<T>.Default)
        { }

        public ConcurrentHashSet(IEqualityComparer<T> equalityComparer)
        {
            _dict = new ConcurrentDictionary<T, byte>(equalityComparer ?? EqualityComparer<T>.Default);
        }

        public ConcurrentHashSet(IEnumerable<T> collection) : this(collection, EqualityComparer<T>.Default)
        { }

        public ConcurrentHashSet(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer) : this(equalityComparer)
        {
            this.UnionWith(collection);
        }

        private const byte Value = 1;
        private readonly ConcurrentDictionary<T, byte> _dict;

        public int Count { get { return _dict.Count; } }

        public bool IsReadOnly { get { return false; } }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(T item)
        {
            return _dict.ContainsKey(item);
        }

        public bool TryAdd(T item)
        {
            return _dict.TryAdd(item, Value);
        }

        public bool TryRemove(T item)
        {
            byte value;
            return _dict.TryRemove(item, out value);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            foreach (T item in other)
            {
                byte value;
                _dict.TryGetValue(item, out value);
                if (value != 1) TryAdd(item);
            }
        }

        #region ICollection<T> Members

        void ICollection<T>.Add(T item)
        {
            if (!TryAdd(item))
                throw new ArgumentException("The item was already present in the collection");
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            CoreThrowHelper.CheckNullArg(array, "array");
            var items = _dict.Keys.ToArray();
            items.CopyTo(array, arrayIndex);
        }

        bool ICollection<T>.Remove(T item)
        {
            return TryRemove(item);
        }

        public void RemoveWhere(Func<T, bool> predicate)
        {
            var itemsToRemove = _dict.Keys.ToArray().Where(predicate);

            foreach (var item in itemsToRemove)
            {
                TryRemove(item);
            }
        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return _dict.Keys.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}