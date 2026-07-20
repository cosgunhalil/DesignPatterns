using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>
    /// Edge-triggered observer: warns only when health CROSSES below a threshold,
    /// and clears when it recovers above — so it fires on the transition, not on
    /// every tick spent in the danger zone. Shows an observer holding its own
    /// state independent of the subject.
    /// </summary>
    public sealed class LowHealthWarning : IObserver<HealthChanged>
    {
        private readonly float _threshold;
        private bool _warned;

        public LowHealthWarning(float threshold = 0.3f)
        {
            _threshold = threshold;
        }

        public void Receive(HealthChanged notification)
        {
            var low = notification.Fraction <= _threshold && !notification.IsDead;

            if (low && !_warned)
            {
                _warned = true;
                Debug.Log($"<color=red>[LowHealth]</color> WARNING — health at {notification.Fraction:P0}!");
            }
            else if (!low && _warned)
            {
                _warned = false;
                Debug.Log("<color=green>[LowHealth]</color> recovered above threshold.");
            }
        }
    }
}
