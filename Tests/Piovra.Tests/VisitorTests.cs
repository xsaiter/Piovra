using Xunit;

namespace Piovra.Tests;

public class VisitorTests {
    public class PrintVisitor : IVisitor<E1, string>, IVisitor<E2, string> {
        public string Visit(E1 element) => element.Ask();
        public string Visit(E2 element) => element.Ask();
    }

    public class E1 : Visitable<E1, string> {
        public string Ask() => "e1";
    }

    public class E2 : Visitable<E2, string> {
        public string Ask() => "e2";
    }

    [Fact]
    public void Test() {
        var visitor = new PrintVisitor();

        var e1 = new E1();
        var r1 = e1.Accept(visitor);
        Assert.Equal("e1", r1);

        var e2 = new E2();
        var r2 = e2.Accept(visitor);
        Assert.Equal("e1", r1);
    }
}
