namespace Piovra.Geo {
    public class Point {
        public Point(double x, double y) => (X, Y) = (x, y);        
        public double X { get; }        
        public double Y { get; }

        public static double CrossProduct(Point a, Point b, Point c) => (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);        

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
}