using System;

namespace Piovra.Graphs;

public class PrimMST<V>(IWeightedGraph<V> g) where V : IEquatable<V> {
    readonly IWeightedGraph<V> _g = g;

    public MST<V> Execute() {
        var mst = new MST<V>();
        return mst;
    }
}
