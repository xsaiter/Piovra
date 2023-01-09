using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Piovra.Tests;

public class RBTreeTests {
    [Fact]
    public void Test() {
        var tree = new RBTree<int, string>();
        tree.Add(1, "a");
        tree.Add(2, "b");
        tree.Add(3, "c");
        tree.Add(4, "d");
        tree.Add(5, "e");
        tree.Add(6, "f");

        var res = tree.GetValue(6);

        var t = tree;
    }
}
