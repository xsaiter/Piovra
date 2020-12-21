using System.Collections.Generic;

namespace Piovra.Ds {
    public class Trie {
        public Node Root { get; } = new Node();

        public void AddWords(IEnumerable<string> words) {
            foreach (var word in words) {
                AddWord(word);
            }
        }

        public void AddWord(string word) {
            if (word.Empty()) return;
            var node = Root;
            foreach (var c in word) {
                node = Walk(node, c);
            }
            node.IsLeaf = true;
        }

        public bool ContainsWord(string word) {
            var node = FindNode(word);
            return node?.IsLeaf == true;
        }

        public int GetCountPrefix(string prefix) {
            var node = FindNode(prefix);
            return node != null ? node.PrefixCount : 0;
        }

        Node FindNode(string s) {
            if (s.Empty()) return null;
            var node = Root;
            foreach (var c in s) {
                if (node.HasChild(c)) {
                    node = node.GetChild(c);
                } else {
                    return null;
                }
            }
            return node;
        }

        static Node Walk(Node node, char c) {
            if (node.HasChild(c)) {
                var found = node.GetChild(c);
                ++found.PrefixCount;
                return found;
            }
            var child = new Node { 
                PrefixCount = 1 
            };
            node.Children.Add(c, child);
            return child;
        }

        public class Node {
            public Dictionary<char, Node> Children { get; } = new Dictionary<char, Node>();
            public bool IsLeaf { get; set; }
            public int PrefixCount { get; set; }
            public bool HasChild(char key) => Children.ContainsKey(key);
            public Node GetChild(char key) => Children[key];
        }
    }
}
