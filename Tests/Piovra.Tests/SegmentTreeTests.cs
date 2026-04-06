using Xunit;

namespace Piovra.Tests;

public class SegmentTreeTests {
    [Fact]
    public void Test() {
        var a = new List<int> { 5, 8, 6, 3, 2, 7, 2, 6 };
        var root = new Ds.SegmentTree.SumNode<int>(0, a.Count - 1);
        root.Build(a);
        var tree = new Ds.SegmentTree.Tree<int>(root);
        var ans = tree.Query(1, 2);

        Assert.Equal(15, ans);
    }
}
