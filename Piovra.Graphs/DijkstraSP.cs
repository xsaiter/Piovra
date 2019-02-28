using System;

namespace Piovra.Graphs {
    public class DijkstraSP<V, G> where V : IEquatable<V> {
        readonly IWeightedGraph<V> _g;

        public DijkstraSP(IWeightedGraph<V> g) {
            _g = g;
        }

        public ShortestPath Execute() {
            var res = new ShortestPath();
            return res;
        }
    }
}