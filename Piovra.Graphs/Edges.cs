using System;

namespace Piovra.Graphs {
    public interface IEdge<V> : IEquatable<IEdge<V>>
        where V : IEquatable<V> {
        Node<V> Head { get; }
        Node<V> Tail { get; }
    }

    public class Edge<V> : IEdge<V> where V : IEquatable<V> {
        public static Edge<V> Of(V head, V tail) => new Edge<V>(head, tail);

        Edge(V head, V tail) {
            Head = Node<V>.Of(head);
            Tail = Node<V>.Of(tail);
        }

        public Node<V> Head { get; }
        public Node<V> Tail { get; }

        public bool Equals(IEdge<V> other) {
            if (other == null) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Head.Equals(other.Head) && Tail.Equals(other.Tail);
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
            return Equals(obj as Edge<V>);
        }

        public override int GetHashCode() {
            return Utils.Enumerate(Head, Tail).ComputeHash();
        }
    }

    public class WeightedEdge<V> : IEdge<V> where V : IEquatable<V> {
        public static WeightedEdge<V> Of(V head, V tail, double weight) =>
            new WeightedEdge<V>(Edge<V>.Of(head, tail), weight);

        WeightedEdge(Edge<V> edge, double weight) {
            Edge = edge;
            Weight = weight;
        }

        Edge<V> Edge { get; }
        public Node<V> Head => Edge.Head;
        public Node<V> Tail => Edge.Tail;
        public double Weight { get; }

        public bool Equals(IEdge<V> other) {
            if (other == null) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            var otherObj = (WeightedEdge<V>)other;
            return Edge.Equals(otherObj.Edge);
        }

        public override bool Equals(object obj) {
            return Equals(obj as WeightedEdge<V>);
        }

        public override int GetHashCode() {
            return Utils.EnumerateObjects(Edge, Weight).ComputeHash();
        }
    }
}