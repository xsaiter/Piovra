using System.Linq;
using System.Collections.Generic;

namespace Piovra.Geo;

public class GrahamScanConvexHullStrategy : ICreateConvexHullStrategy {
    public ConvexHull Create(IEnumerable<Point> points) {
        var result = new ConvexHull();

        var n = points.Count();

        if (n < 4) {
            points.Foreach(_ => result.AddPoint(_));
            return result;
        }

        var pointList = points.ToList();

        var posMinY = 0;
        var minY = pointList[0].Y;

        for (var i = 1; i < n; ++i) {
            if (pointList[i].Y < minY) {
                minY = pointList[i].Y;
                posMinY = i;
            }
        }

        (pointList[posMinY], pointList[0]) = (pointList[0], pointList[posMinY]);

        return result;
    }
}
