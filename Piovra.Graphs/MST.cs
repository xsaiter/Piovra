using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Graphs {
    public class MST<V> where V : IEquatable<V> {
        public List<WeightedEdge<V>> Edges { get; } = new List<WeightedEdge<V>>();
    }

    public class KruskalMST<V> where V : IEquatable<V> {
        readonly WeightedDirectedGraph<V> _g;

        public KruskalMST(WeightedDirectedGraph<V> g) {
            _g = g;
        }

        public MST<V> Execute() {
            var res = new MST<V>();

            var orderedEdges = _g.AllEdges().OrderBy(e => e.Weight);

            var ds = new DisjointSet<Node<V>>();

            //foreach (var edge in orderedEdges) {
            //    ds.Add(edge.Head);
            //    ds.Add(edge.Tail);
            //}

            foreach (var edge in orderedEdges) {
                var head = ds.Find(edge.Head);
                var tail = ds.Find(edge.Tail);

                if (!head.Equals(tail)) {
                    res.Edges.Add(edge);
                    ds.Union(head, tail);
                }
            }

            return res;
        }
    }

    public class PrimMST<V> where V : IEquatable<V> {
        readonly WeightedDirectedGraph<V> _g;

        public PrimMST(WeightedDirectedGraph<V> g) {
            _g = g;
        }

        public MST<V> Execute() {
            var res = new MST<V>();
            return res;
        }
    }
}
