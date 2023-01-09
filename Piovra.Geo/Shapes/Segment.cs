namespace Piovra.Geo;

public class Segment {
    public Segment(Point a, Point b) => (A, B) = (a, b);
    public Point A { get; }
    public Point B { get; }
}