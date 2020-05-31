using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Ds {
    public class BTree<K, V> where K : IComparable<K> {
        public BTree(int degree) {
            if (degree < 2) {
                throw new ArgumentOutOfRangeException(nameof(degree));
            }
            Degree = degree;
            Root = new Node(degree);
            H = 1;
        }

        public int Degree { get; }
        public Node Root { get; set; }
        public int H { get; set; }

        public Entry Search(K key) {
            return SearchRec(Root, key);
        }

        Entry SearchRec(Node x, K key) {
            var pos = x.Entries.TakeWhile(_ => key.Ge(_.Key)).Count();
            if (pos < x.NEntries && x.Entries[pos].Key.Eq(key)) {
                return x.Entries[pos];
            }
            return x.IsLeaf ? null : SearchRec(x.Children[pos], key);
        }

        public void Add(K key, V value) {
            if (!Root.IsMax()) {
                InsertNonFull(Root, key, value);
            } else {
                ++H;
            }
        }

        void Split(Node x, K key, V value) {

        }

        void InsertNonFull(Node x, K key, V value) {

        }

        public class Node {
            public Node(int degree) {
                Degree = degree;
                Children = new List<Node>(degree);
                Entries = new List<Entry>(degree);
            }

            public int Degree { get; }
            public List<Node> Children { get; }
            public List<Entry> Entries { get; }
            public int NEntries => Entries.Count;
            public bool IsLeaf { get; set; }

            public bool IsMax() => NEntries == 2 * Degree - 1;
            public bool IsMin() => NEntries == Degree - 1;
        }

        public class Entry {
            public K Key { get; set; }
            public V Value { get; set; }
        }
    }
}