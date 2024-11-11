using System.Collections.Generic;

namespace Piovra.Geo;

public class ConvexHull {
    public List<Point> Points { get; } = [];

    public void AddPoint(Point point) {
        Points.Add(point);
    }
}
