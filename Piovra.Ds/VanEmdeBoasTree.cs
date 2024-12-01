using System.Collections.Generic;

namespace Piovra.Ds;

public class VanEmdeBoasTree {
    public class Node {
        public Node Parent { get; set; }
        public List<Node> Children { get; set; } = new();
    }
}
