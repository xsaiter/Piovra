namespace Piovra.Geo;

public interface IConvexHullBuilder {
    ConvexHull Create(IEnumerable<Point> points);

    public static IConvexHullBuilder Graham() => new GrahamConvexHullBuilder();
}
