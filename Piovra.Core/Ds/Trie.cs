using System.Collections.Generic;

namespace Piovra.Ds {
    public class Trie {
        public Node Root { get; } = new Node();

        public void Add(string s) {
            foreach (var x in Root.Children) {

            }
        }

        public class Node {
            public string Value { get; set; }
            public char Jump { get; set; }
            public List<Node> Children { get; } = new List<Node>();
            public bool IsLeaf { get; set; }
            public string Leaf { get; set; }
        }
    }
}
