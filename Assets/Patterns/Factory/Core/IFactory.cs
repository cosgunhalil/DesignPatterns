namespace DesignPatterns.Factory
{
    /// <summary>
    /// A creator of products — the "factory" role in its simplest form. A client
    /// depends on this instead of a concrete type or a <c>new</c> expression, so
    /// what gets built can change without the client changing.
    /// </summary>
    /// <typeparam name="TProduct">The type produced.</typeparam>
    public interface IFactory<out TProduct>
    {
        TProduct Create();
    }

    /// <summary>
    /// A creator that needs a construction argument (e.g. a spawn position or a
    /// difficulty level).
    /// </summary>
    /// <typeparam name="TArg">The argument passed to each creation.</typeparam>
    /// <typeparam name="TProduct">The type produced.</typeparam>
    public interface IFactory<in TArg, out TProduct>
    {
        TProduct Create(TArg arg);
    }
}
