using Piovra.Graphs;
using System.Linq;
using Xunit;

namespace Piovra.Tests;

public class GraphTests {
    [Fact]
    public void Test() {
        var g = new UnweightedDirectedGraph<string>();
        g.AddVertex("a");
        g.AddEdge(Edge<string>.Of("b", "c"));
        g.AddEdge(Edge<string>.Of("b", "e"));
        g.AddEdge(Edge<string>.Of("e", "f"));
        g.AddEdge(Edge<string>.Of("f", "d"));

        var node = g.NodeOf("c");
        var node2 = g.NodeOf("f");

        var allNodes = g.AllNodes();
        var nbrs = g.Neighbors(Node<string>.Of("b")).ToList();
        var incidentEdges = g.IncidentEdges(Node<string>.Of("b")).ToList();

        var bfs = new BFS<string>(g);
        var res = bfs.Execute("b");
        var tmp = res;
    }
}
