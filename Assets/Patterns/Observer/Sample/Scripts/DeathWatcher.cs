using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>
    /// Reacts once when health reaches zero. It unsubscribes ITSELF from inside
    /// <see cref="Receive"/> (there's nothing more to watch after death) — which
    /// is safe only because <see cref="Subject{TEvent}.Notify"/> iterates a
    /// snapshot. A subject that looped the live list would corrupt iteration here.
    /// </summary>
    public sealed class DeathWatcher : IObserver<HealthChanged>
    {
        private readonly ISubject<HealthChanged> _health;
        public bool HasDied { get; private set; }

        public DeathWatcher(ISubject<HealthChanged> health)
        {
            _health = health;
        }

        public void Receive(HealthChanged notification)
        {
            if (!notification.IsDead || HasDied)
            {
                return;
            }

            HasDied = true;
            Debug.Log("<color=magenta>[DeathWatcher]</color> the character has died — triggering respawn flow.");
            _health.Unsubscribe(this);
        }
    }
}
