namespace Piovra;

public abstract class Decorator<T>(T element) where T : class {
    public T Element { get; } = element;
    public T OriginalElement() => Element is Decorator<T> element ? element.OriginalElement() : Element;
}
