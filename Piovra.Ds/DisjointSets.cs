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
        if (_nodes.TryGetValue(x, out var value)) {
            return value;
        }
        var newNode = new Node(x);
        _nodes.Add(x, newNode);
        return newNode;
    }

    static void Link(Node x, Node y) {
        if (x.Rank > y.Rank) {
            y.Parent = x;
        } else if (x.Parent == y && x.Rank == y.Rank) {
            y.Rank++;
        }
    }

    public class Node : IEquatable<Node> {
        public Node(T item) {
            Item = item;
            Parent = this;
        }
        public T Item { get; }
        public Node Parent { get; set; }
        public int Rank { get; set; }

        public bool Equals(Node? other) {
            if (other == null) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Item.Equals(other.Item);
        }

        public override bool Equals(object? obj) {
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
