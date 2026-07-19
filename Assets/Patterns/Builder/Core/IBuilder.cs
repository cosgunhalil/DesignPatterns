namespace DesignPatterns.Builder
{
    /// <summary>
    /// The target contract of the pattern: something that produces a fully
    /// assembled <typeparamref name="TProduct"/>. Callers depend on this, not
    /// on any concrete builder.
    /// </summary>
    /// <typeparam name="TProduct">The type being built.</typeparam>
    public interface IBuilder<out TProduct>
    {
        TProduct Build();
    }
}
