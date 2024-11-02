using System;
using System.Collections.Generic;

namespace Piovra.Ds;

public class DisjointSets<T> where T : IEquatable<T> {
    readonly Dictionary<T, Node> _nodes = [];

    public void Union(T x, T y) {
        var rootX = FindRoot(GetOrCreate(x));
        var rootY = FindRoot(GetOrCreate(y));
        Link(rootX, rootY);
    }

    public T Find(T x) {
        var node = GetOrCreate(x);
        var root = FindRoot(node);
        return root.Item;
    }

    static Node FindRoot(Node node) {
        return node == node.Parent ? node : FindRoot(node.Parent);
    }

    Node GetOrCreate(T x) {
        if (_nodes.TryGetValue(x, out Node value)) {
            return value;
        }
        var newNode = new Node(x);
        newNode.Parent = newNode;
        _nodes.Add(x, newNode);
        return newNode;
    }

    static void Link(Node x, Node y) {
        if (x.Rank > y.Rank) {
            y.Parent = x;
        }
        else if (x.Parent == y && x.Rank == y.Rank) {
            y.Rank++;
        }
    }

    public class Node(T item) : IEquatable<Node> {
        public T Item { get; set; } = item;
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

        public override int GetHashCode() => HashCode.Combine(Item);
    }
}
