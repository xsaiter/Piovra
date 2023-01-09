using Piovra.Ds;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Piovra.Tests;

public class BinaryHeapTests {
    [Fact]
    public void Test() {
        var heap = new BinaryHeap<int>(3);
        heap.Add(10);
        heap.Add(5);
        heap.Add(15);

        var top1 = heap.Top();
        heap.Pop();

        var top2 = heap.Top();
        heap.Pop();

        var top3 = heap.Top();
    }
}
