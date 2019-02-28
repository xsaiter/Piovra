using System;
using System.Collections.Generic;

namespace Piovra.Graphs {
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

    public class Result<V> where V : IEquatable<V> {
        public Result(Node<V> source) {
            Source = source;
        }
        public Node<V> Source { get; set; }
        public List<Item> Items { get; set; }
    }
}