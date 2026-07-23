namespace DesignPatterns.Decorator
{
    /// <summary>
    /// Exposes the component a decorator wraps, so a chain can be inspected or
    /// unwrapped (useful in tests and debugging). A decorator both <em>is-a</em>
    /// component (it implements the component's own interface) and <em>has-a</em>
    /// component (the one it wraps) — that dual nature is exactly what lets
    /// decorators nest to any depth.
    /// </summary>
    /// <typeparam name="TComponent">The wrapped component's type.</typeparam>
    public interface IDecorator<out TComponent>
    {
        TComponent Component { get; }
    }
}
