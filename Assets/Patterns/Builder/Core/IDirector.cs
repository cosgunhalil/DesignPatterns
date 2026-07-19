namespace DesignPatterns.Builder
{
    /// <summary>
    /// A Director encapsulates a reusable construction recipe: it drives a
    /// builder through a fixed sequence of steps to produce one specific
    /// configuration. This separates "how to assemble a particular preset" from
    /// "how to build in general" (which lives in the builder).
    /// </summary>
    /// <typeparam name="TBuilder">The builder the recipe drives.</typeparam>
    /// <typeparam name="TProduct">The type produced.</typeparam>
    public interface IDirector<in TBuilder, out TProduct>
    {
        TProduct Construct(TBuilder builder);
    }
}
