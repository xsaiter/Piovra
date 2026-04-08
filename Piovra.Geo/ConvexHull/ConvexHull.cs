namespace Piovra.Geo;

public class ConvexHull {
    readonly List<Point> _points = [];

    public IReadOnlyList<Point> Points => _points;
    public bool IsEmpty => _points.Count == 0;

    public void AddPoint(Point point) {
        Requires.NotNull(point);
        _points.Add(point);
    }
}
