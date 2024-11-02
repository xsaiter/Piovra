using System;
using System.Collections.Generic;

namespace Piovra.Graphs;

public enum Colors {
    White,
    Black,
    Gray
}

public class Item {
    public Colors Color { get; set; }
    public int Distance { get; set; }
    public const int NO_DISTANCE = -1;
}

public class Result<V>(Node<V> source) where V : IEquatable<V> {
    public Node<V> Source { get; set; } = source;
    public List<Item> Items { get; set; } = [];
}
