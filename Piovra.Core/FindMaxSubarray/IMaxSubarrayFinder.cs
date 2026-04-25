using System.Collections.Generic;

namespace Piovra;

public interface IMaxSubarrayFinder {
    FindMaxSubarrayResult FindMaxSubarray<T>(IEnumerable<T> a);
}
