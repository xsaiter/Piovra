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

        public Node Root { get; private set; }

        public int H { get; private set; }

        public Entry Search(K key) => SearchImpl(Root, key);        

        Entry SearchImpl(Node x, K key) {
            var pos = x.Entries.TakeWhile(_ => key.Ge(_.Key)).Count();
            if (pos < x.NEntries && x.Entries[pos].Key.Eq(key)) {
                return x.Entries[pos];
            }
            return x.IsLeaf ? null : SearchImpl(x.Children[pos], key);
        }

        public void Add(K key, V value) {
            if (!Root.HasReachedMaxEntries) {
                InsertNonFull(this.Root, key, value);
            } else {
                var oldRoot = Root;
                Root = new Node(Degree);
                Root.Children.Add(oldRoot);
                SplitChild(Root, 0, oldRoot);
                InsertNonFull(Root, key, value);
                H++;
            }
        }

        void SplitChild(Node parent, int index, Node toSplit) {
            var newNode = new Node(Degree);

            parent.Entries.Insert(index, toSplit.Entries[Degree - 1]);
            parent.Children.Insert(index + 1, newNode);

            newNode.Entries.AddRange(toSplit.Entries.GetRange(Degree, Degree - 1));

            toSplit.Entries.RemoveRange(Degree - 1, Degree);

            if (!toSplit.IsLeaf) {
                newNode.Children.AddRange(toSplit.Children.GetRange(Degree, Degree));
                toSplit.Children.RemoveRange(Degree, Degree);
            }
        }

        void InsertNonFull(Node node, K key, V value) {
            var positionToInsert = node.Entries.TakeWhile(entry => key.CompareTo(entry.Key) >= 0).Count();
            if (node.IsLeaf) {
                node.Entries.Insert(positionToInsert, new Entry { Key = key, Value = value });
            } else {
                var child = node.Children[positionToInsert];
                if (child.HasReachedMaxEntries) {
                    SplitChild(node, positionToInsert, child);
                    if (key.CompareTo(node.Entries[positionToInsert].Key) > 0) {
                        positionToInsert++;
                    }
                }
                InsertNonFull(node.Children[positionToInsert], key, value);
            }
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

            public bool HasReachedMaxEntries => Entries.Count == 2 * Degree - 1;

            public bool HasReachedMinEntries => Entries.Count == Degree - 1;
        }

        public class Entry {
            public K Key { get; set; }
            public V Value { get; set; }
        }
    }
}