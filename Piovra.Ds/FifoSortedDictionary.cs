using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Piovra.Ds {
    public class FifoSortedDictionary<Key, Value> : IDictionary<Key, Value> where Key : notnull {
        readonly SortedDictionary<Key, Value> _dict;
        readonly Dictionary<Key, Value> _dictKeys;

        public Value this[Key key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<Key> Keys => throw new NotImplementedException();

        public ICollection<Value> Values => throw new NotImplementedException();

        public int Count => _dict.Count;

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(Key key, Value value) {
            _dict.Add(key, value);
        }

        public void Add(KeyValuePair<Key, Value> item) {
            _dict.Add(item.Key, item.Value);
        }

        public void Clear() {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<Key, Value> item) {
            throw new NotImplementedException();
        }

        public bool ContainsKey(Key key) {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<Key, Value>[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<Key, Value>> GetEnumerator() {
            return _dictKeys.GetEnumerator();
        }

        public bool Remove(Key key) {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<Key, Value> item) {
            throw new NotImplementedException();
        }

        public bool TryGetValue(Key key, [MaybeNullWhen(false)] out Value value) {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }

        public class Enumerator : IEnumerator<KeyValuePair<Key, Value>> {
            readonly Dictionary<Key, int> _keys;
            public Enumerator(Dictionary<Key, int> keys) {
                _keys = keys;
            }

            public KeyValuePair<Key, Value> Current => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose() {
            }

            public bool MoveNext() {
                throw new NotImplementedException();
            }

            public void Reset() {
                throw new NotImplementedException();
            }
        }
    }
}