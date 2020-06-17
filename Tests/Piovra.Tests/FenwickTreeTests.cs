using Piovra.Ds;
using Xunit;

namespace Piovra.Tests {
    public class FenwickTreeTests {
        [Fact]
        public void Test() {
            var tree = new FenwickTree(10);
            tree.
                Update(1, 1).
                Update(2, 3).
                Update(3, 4).
                Update(4, 8).
                Update(5, 6).
                Update(6, 1).
                Update(7, 4).
                Update(8, 2);

            var sum = tree.Sum(2, 4);

            Assert.Equal(15, sum);
        }
    }
}