using System;

namespace DesignPatterns.Component
{
    /// <summary>
    /// Convenience base for components: it tracks the owning <see cref="Entity"/>
    /// so a component can reach its siblings via <c>Owner.GetComponent&lt;T&gt;()</c>.
    /// Reaching siblings through the owner (rather than holding direct references)
    /// is what keeps domains decoupled — a component asks the entity "is there a
    /// physics component?" instead of being handed one.
    /// </summary>
    public abstract class EntityComponent : IComponent
    {
        /// <summary>The entity this component belongs to, or null while detached.</summary>
        public Entity Owner { get; private set; }

        public virtual void Attach(Entity owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public virtual void Detach()
        {
            Owner = null;
        }
    }
}
