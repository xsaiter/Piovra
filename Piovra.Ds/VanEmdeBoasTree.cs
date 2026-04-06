namespace Piovra.Ds;

public class VanEmdeBoasTree {
    public class Node(Node parent) {
        public Node Parent { get; set; } = parent;
        public List<Node> Children { get; set; } = [];
    }
}
