using System.Xml.Linq;

namespace Piovra.Svg {
    public class Point {
        public Point(int x, int y) => (X, Y) = (x, y);
        public int X { get; set; }
        public int Y { get; set; }
    }

    public abstract class Shape : Drawing {
        protected Shape(Style style) => Style = style;
        public Style Style { get; set; }
        protected void AppendStyle(XElement x) => Style?.WriteTo(x);
    }

    public class Circle : Shape {
        public Circle(Point center, int radius, Style style = null) : base(style) =>
            (Center, Radius) = (center, radius);

        public Point Center { get; }
        public int Radius { get; }

        public override XElement ToSvg() {
            var x = NewXElement("circle",
                new XAttribute("cx", Center.X),
                new XAttribute("cy", Center.Y),
                new XAttribute("r", Radius));
            AppendStyle(x);
            return x;
        }
    }

    public class Rect : Shape {
        public Rect(Point position, int w, int h, Style style = null) : base(style) =>
            (Position, W, H) = (position, w, h);

        public Point Position { get; }
        public int W { get; }
        public int H { get; }

        public override XElement ToSvg() {
            var x = NewXElement("rect",
                new XAttribute("x", Position.X),
                new XAttribute("y", Position.Y),
                new XAttribute("width", W),
                new XAttribute("height", H));
            AppendStyle(x);
            return x;
        }
    }
}