using System;
using System.Linq;
using Piovra.Ds;

namespace Piovra.Graphs;

public class KruskalMST<V>(IWeightedGraph<V> g) where V : IEquatable<V> {
    readonly IWeightedGraph<V> _g = g;

    public MST<V> Execute() {
        var mst = new MST<V>();

        var orderedEdges = _g.AllEdges().OrderBy(e => e.Weight);

        var ds = new DisjointSets<Node<V>>();

        foreach (var edge in orderedEdges) {
            var head = ds.Find(edge.Head);
            var tail = ds.Find(edge.Tail);

            if (!head.Equals(tail)) {
                mst.Edges.Add(edge);
                ds.Union(head, tail);
            }
        }

        return mst;
    }
}
