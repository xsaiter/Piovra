using System;
using System.Collections.Generic;

namespace Piovra.Graphs;
public class ShortestPath {
}

public class Route<V> where V : IEquatable<V> {
    public List<V> Nodes { get; set; } = [];
}
