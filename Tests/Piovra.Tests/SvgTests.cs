using Piovra.Svg;
using Xunit;

namespace Piovra.Tests {
    public class SvgTests {       
        [Fact]
        public void Test() {
            var c1 = new Circle(new Point(10, 20), 10);
            var c2 = new Circle(new Point(50, 50), 40, new Style { FillColor = Color.RED });

            var r1 = new Rect(new Point(100, 100), 30, 30);

            var svg = SVG.MakeRoot(400, 400);
            svg.AddShape(c1);
            svg.AddShape(c2);
            svg.AddShape(r1);

            var image = Image.Make(svg);

            Assert.True(image.Xml != null);
        }
    }
}
