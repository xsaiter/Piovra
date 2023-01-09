using System;
using System.Collections.Generic;
using System.Linq;

namespace Piovra.Graphs;

public class BFS<V> where V : IEquatable<V> {
    readonly IGraph<V> _g;

    public BFS(IGraph<V> g) {
        _g = g;
    }

    public Result<V> Execute(V source) {
        var s = _g.NodeOf(source);

        var res = new Result<V>(s);

        var items = Utils.AllocateList(_g.NV, () => new Item());

        foreach (var node in _g.AllNodes()) {
            if (node.Id != s.Id) {
                var ni = items[node.Id];
                ni.Color = Colors.White;
                ni.Distance = Item.NO_DISTANCE;
            }
        }

        var si = items[s.Id];
        si.Color = Colors.Gray;
        si.Distance = 0;

        var q = new Queue<Node<V>>();
        q.Enqueue(s);

        while (q.Any()) {
            var u = q.Dequeue();
            var ui = items[u.Id];
            foreach (var v in _g.Neighbors(u)) {
                var vi = items[v.Id];
                if (vi.Color == Colors.White) {
                    vi.Color = Colors.Gray;
                    vi.Distance = ui.Distance + 1;
                    q.Enqueue(v);
                }
            }
            ui.Color = Colors.Black;
        }

        res.Items = items;

        return res;
    }
}