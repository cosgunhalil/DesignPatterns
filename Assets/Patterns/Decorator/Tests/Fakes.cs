namespace DesignPatterns.Decorator.Tests
{
    /// <summary>A component interface unrelated to the sample, to prove Decorator&lt;T&gt; is domain-agnostic.</summary>
    internal interface IGreeter
    {
        string Greet();
    }

    internal sealed class PlainGreeter : IGreeter
    {
        public string Greet() => "hi";
    }

    /// <summary>A decorator built on the generic base for the fake component type.</summary>
    internal sealed class LoudGreeter : Decorator<IGreeter>, IGreeter
    {
        public LoudGreeter(IGreeter inner) : base(inner)
        {
        }

        public string Greet() => Component.Greet().ToUpperInvariant() + "!";
    }
}
