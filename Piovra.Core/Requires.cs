using System;

namespace Piovra;

public static class Requires {
    public static T CheckNotNull<T>(T paramValue, string paramName) where T : class =>
        paramValue ?? throw new ArgumentNullException(paramName);

    public static string NotNullOrEmpty(string paramValue, string paramName) =>
        paramValue.NonEmpty() ? paramValue : throw new ArgumentNullException(paramName);

    public static void EnsureOutOfRange(Func<bool> failCondition, string paramName) {
        if (failCondition()) {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
}
