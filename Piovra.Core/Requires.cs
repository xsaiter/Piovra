using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Piovra;

public static class Requires {
    public const string ASSERTION_FAILED = "Assertion failed";
    public const string DEFAULT_NAME = "";
    public const string DEFAULT_DETAILS = "";

    public static void True([DoesNotReturnIf(false)] bool condition, string message = ASSERTION_FAILED) {
        if (!condition) {
            throw new Exception(message);
        }
    }

    [return: NotNull]
    public static T AsNotNull<T>([NotNull] T? value,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : class {

        True(value is not null, $"{name} must not be null");
        return value;
    }

    [return: NotNull]
    public static string AsNotNullOrEmpty(string value,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        True(value.NonEmpty(), $"{name} must not be null or empty");
        return value;
    }

    public static void NotNull<T>([NotNull] T value,
        string details = DEFAULT_DETAILS, [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        True(value is not null, $"{name} must not be null, {nameof(details)}: {details}");
    }
}
