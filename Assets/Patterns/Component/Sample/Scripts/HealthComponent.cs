using UnityEngine;

namespace DesignPatterns.Component.Sample
{
    /// <summary>
    /// An entirely separate domain living on the same entity. It shares no code
    /// with movement or AI — the entity spans "can move" and "can be hurt"
    /// without a single class that mixes the two. Adding or removing it changes
    /// what the entity can do, at runtime.
    /// </summary>
    public sealed class HealthComponent : EntityComponent
    {
        public int Max { get; }
        public int Current { get; private set; }
        public bool IsAlive => Current > 0;

        public HealthComponent(int max)
        {
            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount) => Current = Mathf.Clamp(Current - amount, 0, Max);

        public void Heal(int amount) => Current = Mathf.Clamp(Current + amount, 0, Max);
    }
}
