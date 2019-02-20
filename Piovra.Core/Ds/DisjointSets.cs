﻿using System;
using System.Collections.Generic;

namespace Piovra.Ds {
    public class DisjointSets<T> where T : IEquatable<T> {
        readonly Dictionary<T, Node> _nodes = new Dictionary<T, Node>();

        public void Union(T x, T y) {
            var rootX = FindRoot(GetOrCreate(x));
            var rootY = FindRoot(GetOrCreate(y));
            Link(rootX, rootY);
        }

        public T Find(T x) {
            var root = FindRoot(GetOrCreate(x));
            return root.Item;
        }

        Node FindRoot(Node node) {
            if (node == node.Parent) {
                return node;
            }
            return FindRoot(node.Parent);
        }             

        Node GetOrCreate(T x) {
            if (_nodes.ContainsKey(x)) {
                return _nodes[x];
            }
            var newNode = new Node(x);
            newNode.Parent = newNode;
            _nodes.Add(x, newNode);
            return newNode;
        }

        void Link(Node x, Node y) {
            if (x.Rank > y.Rank) {
                y.Parent = x;
            } else if (x.Parent == y) {
                if (x.Rank == y.Rank) {
                    y.Rank++;
                }
            }
        }

        public class Node : IEquatable<Node> {
            public Node(T item) => Item = item;
            public T Item { get; set; }
            public int Rank { get; set; }
            public Node Parent { get; set; }

            public bool Equals(Node other) {
                if (other == null) {
                    return false;
                }
                if (ReferenceEquals(this, other)) {
                    return true;
                }
                return Item.Equals(other.Item);
            }

            public override bool Equals(object obj) {
                if (obj == null) {
                    return false;
                }
                if (ReferenceEquals(this, obj)) {
                    return true;
                }
                if (obj.GetType() != GetType()) {
                    return false;
                }
                return Equals(obj as Node);
            }

            public override int GetHashCode() {
                return Item.GetHashCode();
            }
        }
    }
}