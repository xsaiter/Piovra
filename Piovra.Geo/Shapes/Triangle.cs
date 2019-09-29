namespace Piovra.Geo {

    public class Triangle {
        public Triangle(Point a, Point b, Point c) => (A, B, C) = (a, b, c);
        public Point A { get; }
        public Point B { get; }
        public Point C { get; }
    }
}