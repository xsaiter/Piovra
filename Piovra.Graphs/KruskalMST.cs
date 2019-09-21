﻿using System;
using System.Linq;
using Piovra.Ds;

namespace Piovra.Graphs {
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
}