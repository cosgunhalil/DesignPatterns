using System.Collections.Generic;

namespace DesignPatterns.Builder
{
    /// <summary>
    /// Generic fluent builder base. The self-referencing type parameter
    /// (<typeparamref name="TSelf"/>, the "curiously recurring template
    /// pattern") lets fluent methods declared in subclasses return the concrete
    /// builder type instead of this base — so chaining like
    /// <c>new FooBuilder().WithA(..).WithB(..)</c> stays strongly typed with no
    /// per-method overriding. That reuse is the whole point of the base.
    ///
    /// It also seals the build flow as a template method: <see cref="Build"/>
    /// runs <see cref="Validate"/>, throws <see cref="BuilderValidationException"/>
    /// if anything was reported, and otherwise returns <see cref="BuildCore"/>.
    /// Subclasses implement only <see cref="Validate"/> and <see cref="BuildCore"/>.
    /// </summary>
    /// <typeparam name="TSelf">The concrete builder type (pass the subclass itself).</typeparam>
    /// <typeparam name="TProduct">The type being built.</typeparam>
    public abstract class Builder<TSelf, TProduct> : IBuilder<TProduct>
        where TSelf : Builder<TSelf, TProduct>
    {
        private readonly List<string> _errors = new();

        /// <summary>This instance typed as the concrete builder; fluent methods `return Self;`.</summary>
        protected TSelf Self => (TSelf)this;

        public TProduct Build()
        {
            _errors.Clear();
            Validate();

            if (_errors.Count > 0)
            {
                throw new BuilderValidationException(_errors.ToArray());
            }

            return BuildCore();
        }

        /// <summary>Report a validation problem from <see cref="Validate"/>. Multiple calls accumulate.</summary>
        protected void AddError(string message) => _errors.Add(message);

        /// <summary>
        /// Check configured state and <see cref="AddError"/> for each problem.
        /// Default: nothing to validate. Never throws — <see cref="Build"/> owns throwing.
        /// </summary>
        protected virtual void Validate()
        {
        }

        /// <summary>Assemble the product. Only called after validation passes.</summary>
        protected abstract TProduct BuildCore();
    }
}
