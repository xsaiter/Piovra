using System;

namespace Piovra.Graphs;

public class DFS<V> where V : IEquatable<V> {
    readonly IGraph<V> _g;

    public DFS(IGraph<V> g) {
        _g = g;
    }

    public Result<V> Execute(V source) {
        var s = _g.NodeOf(source);

        var res = new Result<V>(s);

        return res;
    }
}