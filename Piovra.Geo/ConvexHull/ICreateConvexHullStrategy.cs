using System.Collections.Generic;

namespace Piovra.Geo;

public interface ICreateConvexHullStrategy {
    ConvexHull Create(IEnumerable<Point> points);
}
