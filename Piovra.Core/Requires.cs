using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace Piovra;

public static class Requires {
    const string DEFAULT_NAME = "";
    const string DEFAULT_DETAILS = "";

    public static void True([DoesNotReturnIf(false)] bool condition, string details = DEFAULT_DETAILS, string name = DEFAULT_NAME) {
        if (!condition) {
            throw new ArgumentException(
                message: PrettyMessage(details, "must be true"),
                paramName: name);
        }
    }

    [return: NotNull]
    public static T NotNull<T>([NotNull] T? value, string details = DEFAULT_DETAILS,
       [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : class {

        if (value is null) {
            throw new ArgumentNullException(
                paramName: name,
                message: PrettyMessage(details, "must not be null"));
        }

        return value;
    }

    [return: NotNull]
    public static string NotNullOrEmpty(string value,
        string details = DEFAULT_DETAILS, [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        if (string.IsNullOrEmpty(value)) {
            throw new ArgumentNullException(
                paramName: name,
                message: PrettyMessage(details, "must not be null or empty"));
        }

        return value;
    }

    [return: NotNull]
    public static string NotNullOrWhiteSpace(string value,
        string details = DEFAULT_DETAILS, [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        if (string.IsNullOrWhiteSpace(value)) {
            throw new ArgumentNullException(
                paramName: name,
                message: PrettyMessage(details, "must not be null or white space"));
        }

        return value;
    }

    public static T Positive<T>(T value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        if (value <= T.Zero) {
            throw new ArgumentOutOfRangeException(
                paramName: name,
                actualValue: value,
                message: PrettyMessage(details, "must be positive (> 0)"));
        }

        return value;
    }

    public static T Negative<T>(T value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        if (value >= T.Zero) {
            throw new ArgumentOutOfRangeException(
                paramName: name,
                actualValue: value,
                message: PrettyMessage(details, "must be negative (< 0)"));
        }

        return value;
    }

    public static T Zero<T>(T value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        if (value != T.Zero) {
            throw new ArgumentOutOfRangeException(
                paramName: name,
                actualValue: value,
                message: PrettyMessage(details, "must be zero"));
        }

        return value;
    }

    public static T NonPositive<T>(T value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        if (value > T.Zero) {
            throw new ArgumentOutOfRangeException(
                name,
                actualValue: value,
                PrettyMessage(details, "must be non-positive (<= 0)"));
        }

        return value;
    }

    public static T NonNegative<T>(T value, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        if (value < T.Zero) {
            throw new ArgumentOutOfRangeException(
                name,
                actualValue: value,
                PrettyMessage(details, "must be non-negative (>= 0)"));
        }

        return value;
    }

    public static T ClosedRange<T>(T value, T min, T max, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        ValidRange(min, max);

        if (value < min || value > max) {
            throw new ArgumentOutOfRangeException(
                name,
                actualValue: value,
                message: PrettyMessage(details, $"must be in closed range [{min}..{max}]"));
        }

        return value;
    }

    public static T OpenRange<T>(T value, T min, T max, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        ValidRange(min, max);

        if (value <= min || value >= max) {
            throw new ArgumentOutOfRangeException(
                name,
                actualValue: value,
                message: PrettyMessage(details, $"must be in open range ({min}..{max})"));
        }

        return value;
    }

    public static T LeftClosedRange<T>(T value, T min, T max, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        ValidRange(min, max);

        if (value < min || value >= max) {
            throw new ArgumentOutOfRangeException(
                paramName: name,
                actualValue: value,
                message: PrettyMessage(details, $"must be in left-closed range [{min}..{max})"));
        }

        return value;
    }

    public static T RightClosedRange<T>(T value, T min, T max, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) where T : INumber<T> {

        ValidRange(min, max);

        if (value <= min || value > max) {
            throw new ArgumentOutOfRangeException(
                paramName: name,
                actualValue: value,
                message: PrettyMessage(details, $"must be in right-closed range ({min}..{max}]"));
        }

        return value;
    }

    public static T In<T>(T value, IEnumerable<T> values, string details = DEFAULT_DETAILS,
        [CallerArgumentExpression(nameof(value))] string name = DEFAULT_NAME) {

        if (!values.Contains(value)) {
            throw new ArgumentException(
                message: PrettyMessage(details, $"must be one of the allowed values: {string.Join(", ", values)}"),
                paramName: name);
        }

        return value;
    }

    public static void ValidRange<T>(T min, T max) where T : INumber<T> {
        if (min > max) {
            throw new ArgumentException(
                message: $"Range minimum '{min}' cannot be greater than maximum '{max}'.");
        }
    }

    static string PrettyMessage(string details, string requirements) {
        return $"{requirements}{FormatDetails()}";

        string FormatDetails() {
            return details.NonEmpty() ? $", Details: {details}" : string.Empty;
        }
    }
}
