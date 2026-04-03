namespace Piovra;

public record Result<T>(T Value) {
    public static Result<T> Of(T value) => new(value);
}
