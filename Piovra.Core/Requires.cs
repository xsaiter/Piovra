using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Piovra;

public static class Requires {
    public const string ASSERTION_FAILED = "Assertion failed";
    public const string DEFAULT_NAME = EMPTY;
    public const string DEFAULT_DETAILS = EMPTY;
    public const string EMPTY = "";

    public static void True([DoesNotReturnIf(false)] bool condition, string message = ASSERTION_FAILED) {
        if (!condition) {
            throw new Exception(message);
        }
    }

    public static void NotNull<T>([NotNull] T value,
        string details = DEFAULT_DETAILS, [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        True(value is not null, PrettyMessage(name, details, $"must not be null"));
    }

    public static void NotNullOrEmpty(string value,
        string details = DEFAULT_DETAILS, [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        True(value.NonEmpty(), PrettyMessage(name, details, "must not be null or empty"));
    }

    [return: NotNull]
    public static T AsNotNull<T>([NotNull] T? value, string details = DEFAULT_DETAILS,
       [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : class {

        NotNull(value: value, details: details, name: name);
        return value;
    }

    [return: NotNull]
    public static string AsNotNullOrEmpty(string value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        NotNullOrEmpty(value: value, details: details, name: name);
        return value;
    }

    static string PrettyMessage(string name, string details, string requirements) {
        return $"{name} {requirements}, {nameof(details)}: {details}";
    }
}
