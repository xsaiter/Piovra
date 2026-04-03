using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

    public static void NotNull<T>([NotNull] T param,
        string details = "", [CallerArgumentExpression(nameof(param))] string paramName = "") {
        if (param is null) {
            throw new Exception($"{paramName} must not be null, {nameof(details)}: {details}");
        }
    }
}
