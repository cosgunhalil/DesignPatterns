using System;
using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>
    /// The concrete subject. It owns the health value and decides when to
    /// broadcast; observers (UI, audio, achievements, death handling) attach
    /// without the health system knowing any of them exist. Health is clamped to
    /// [0, Max], and a change that doesn't move the value broadcasts nothing.
    /// </summary>
    public sealed class HealthSystem : Subject<HealthChanged>
    {
        public int Max { get; }
        public int Current { get; private set; }

        public HealthSystem(int max)
        {
            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max), "Max health must be positive.");
            }

            Max = max;
            Current = max;
        }

        public void TakeDamage(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Damage cannot be negative; use Heal.");
            }

            ApplyDelta(-amount);
        }

        public void Heal(int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Heal cannot be negative; use TakeDamage.");
            }

            ApplyDelta(amount);
        }

        private void ApplyDelta(int delta)
        {
            var previous = Current;
            Current = Mathf.Clamp(Current + delta, 0, Max);

            if (Current == previous)
            {
                return; // e.g. healing at full health, or damage while already dead
            }

            Notify(new HealthChanged { Previous = previous, Current = Current, Max = Max });
        }
    }
}
