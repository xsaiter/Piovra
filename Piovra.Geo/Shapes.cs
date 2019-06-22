namespace Piovra.Geo {
    public class Point {
        public Point(double x, double y) {
            X = x;
            Y = y;
        }
        public double X { get; }
        public double Y { get; }

        public static double CrossProduct(Point a, Point b, Point c) {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        public static Orientations Orientation(Point a, Point b, Point c) {
            var x = CrossProduct(a, b, c);
            return x == 0.0 ? Orientations.Collinear : x > 0.0 ? Orientations.Clockwise : Orientations.Counterclockwise;
        }

        public enum Orientations {
            Clockwise,
            Counterclockwise,
            Collinear
        }
    }

    public class Segment {
        public Segment(Point a, Point b) {
            A = a;
            B = b;
        }
        public Point A { get; }
        public Point B { get; }
    }

    public class Triangle {
        public Triangle(Point a, Point b, Point c) {
            A = a;
            B = b;
            C = c;
        }
        public Point A { get; }
        public Point B { get; }
        public Point C { get; }
    }

    public class Rect {

    }
}