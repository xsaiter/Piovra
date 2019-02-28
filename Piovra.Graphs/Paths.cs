using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Piovra.Graphs {
    public class ShortestPath {

    }

    public class Route<V> where V : IEquatable<V> {
        public List<V> Nodes { get; set; }
    }
}