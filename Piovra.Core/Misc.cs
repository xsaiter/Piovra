using System;

namespace Piovra;

public static class ARG {
    public static T NotNull<T>(T paramValue, string paramName) where T : class =>
    paramValue ?? throw new ArgumentNullException(paramName);

    public static string NotNullOrEmpty(string paramValue, string paramName) =>
    paramValue.NonEmpty() ? paramValue : throw new ArgumentNullException(paramName);

    public static void EnsureOutOfRange(Func<bool> failCondition, string paramName) {
        if (failCondition()) {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
}

public class ASSERT {
    public static void True(bool condition, string message = null) {
        if (!condition) {
            throw new Exception(message);
        }
    }
    public static void False(bool condition, string message = null) => True(!condition, message);
}
