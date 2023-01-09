namespace Piovra;

public abstract class Decorator<T> where T : class {
    protected Decorator(T element) => Element = element;
    public T Element { get; }
    public T OriginalElement() => Element is Decorator<T> element ? element.OriginalElement() : Element;
}
