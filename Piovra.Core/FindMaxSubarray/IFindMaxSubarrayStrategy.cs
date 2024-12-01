using System.Collections.Generic;

namespace Piovra;

public interface IFindMaxSubarrayStrategy {
    FindMaxSubarrayResult FindMaxSubarray<T>(IEnumerable<T> a);
}
