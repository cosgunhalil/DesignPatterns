using System;

namespace DesignPatterns.Decorator
{
    /// <summary>
    /// Base for object decorators: stores the wrapped component and null-checks
    /// it, so concrete decorators don't repeat that boilerplate. A subclass
    /// derives from this AND implements the component interface, forwarding to
    /// <see cref="Component"/> and adding behavior before, after, or around each call.
    ///
    /// Generic over the component type, so one base serves decorators of any
    /// interface — the reusable core of the pattern.
    /// </summary>
    /// <typeparam name="TComponent">The component interface being decorated.</typeparam>
    public abstract class Decorator<TComponent> : IDecorator<TComponent> where TComponent : class
    {
        /// <summary>The wrapped component. Concrete decorators call through this and augment the result.</summary>
        public TComponent Component { get; }

        protected Decorator(TComponent component)
        {
            Component = component ?? throw new ArgumentNullException(nameof(component));
        }
    }
}
