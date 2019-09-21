using System;

namespace Piovra.Graphs {
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