namespace DesignPatterns.Component
{
    /// <summary>
    /// A capability an <see cref="Entity"/> can hold. Each component owns one
    /// domain (movement, health, AI, rendering) and stays ignorant of the
    /// others — the whole point of the pattern is that an entity can span many
    /// domains without those domains coupling to each other.
    ///
    /// <see cref="Attach"/>/<see cref="Detach"/> are lifecycle calls the entity
    /// makes; you rarely call them yourself. Most components derive from
    /// <see cref="EntityComponent"/>, which implements them.
    /// </summary>
    public interface IComponent
    {
        void Attach(Entity owner);

        void Detach();
    }
}
