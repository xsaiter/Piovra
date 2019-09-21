using System;
using System.Collections.Generic;

namespace Piovra.Graphs {
    public class MST<V> where V : IEquatable<V> {
        public List<WeightedEdge<V>> Edges { get; } = new List<WeightedEdge<V>>();
    }
}