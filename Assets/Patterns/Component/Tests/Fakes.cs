namespace DesignPatterns.Component.Tests
{
    /// <summary>A passive component with no per-frame cost.</summary>
    internal sealed class TagComponent : EntityComponent
    {
        public string Label { get; set; }
    }

    /// <summary>An updatable component that counts its ticks.</summary>
    internal sealed class CounterComponent : EntityComponent, IUpdatable
    {
        public int Ticks { get; private set; }

        public void Update(float deltaTime) => Ticks++;
    }

    /// <summary>Removes itself from its owner on the first tick — exercises safe removal during Update.</summary>
    internal sealed class SelfRemovingComponent : EntityComponent, IUpdatable
    {
        public int Ticks { get; private set; }

        public void Update(float deltaTime)
        {
            Ticks++;
            Owner.RemoveComponent<SelfRemovingComponent>();
        }
    }
}
