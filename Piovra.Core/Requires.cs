using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Piovra;

public static class Requires {
    [return: NotNull]
    public static T AsNotNull<T>([NotNull] T? value, [CallerArgumentExpression(nameof(value))] string name = "")
        where T : class {
        True(value is not null, $"{name} must not be null");
        return value;
    }

    [return: NotNull]
    public static string AsNotNullOrEmpty(string value, string name) {
        True(value.NonEmpty(), $"{name} must not be null or empty");
        return value;
    }

    public static void NotNull<T>([NotNull] T value,
        string details = "", [CallerArgumentExpression(nameof(value))] string name = "") {

        True(value is not null, $"{name} must not be null, {nameof(details)}: {details}");
    }

    public static void True([DoesNotReturnIf(false)] bool condition, string message = "Assertion failed") {
        if (!condition) {
            throw new Exception(message);
        }
    }
}
