namespace Piovra;

public class Result<T> {
    public T Value { get; set; }
    public static Result<T> Of(T value) => new() { Value = value };
}
