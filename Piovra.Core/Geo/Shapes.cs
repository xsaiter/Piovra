namespace Piovra.Geo {
    public class Point {
        public Point(double x, double y) {
            X = x;
            Y = y;
        }
        public double X { get; }
        public double Y { get; }
    }

    public class Segment {
        public Segment(Point a, Point b) {
            A = a;
            B = b;
        }
        public Point A { get; }
        public Point B { get; }
    }

    public class Triangle { }

    public class Rect {

    }
}