using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Graphs {
    public class MST<V> where V : IEquatable<V> {
        public List<WeightedEdge<V>> Edges { get; } = new List<WeightedEdge<V>>();
    }

    public class KruskalMST<V> where V : IEquatable<V> {        
        readonly IWeightedGraph<V> _g;

        public KruskalMST(IWeightedGraph<V> g) {
            _g = g;
        }

        public MST<V> Execute() {
            var res = new MST<V>();

            var orderedEdges = _g.AllEdges().OrderBy(e => e.Weight);

            var ds = new DisjointSets<Node<V>>();            

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
        readonly IWeightedGraph<V> _g;

        public PrimMST(IWeightedGraph<V> g) {
            _g = g;
        }

        public MST<V> Execute() {
            var res = new MST<V>();
            return res;
        }
    }
}
