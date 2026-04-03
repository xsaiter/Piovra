using System.Collections.Generic;

namespace Piovra.Geo;

public interface IConvexHullBuilder {
    ConvexHull Create(IEnumerable<Point> points);
}
