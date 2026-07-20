using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>UI-ish observer: renders a textual health bar on every change.</summary>
    public sealed class HealthBarView : IObserver<HealthChanged>
    {
        private const int SegmentCount = 10;

        public void Receive(HealthChanged notification)
        {
            var filled = Mathf.RoundToInt(notification.Fraction * SegmentCount);
            var bar = new string('#', filled) + new string('-', SegmentCount - filled);
            Debug.Log($"<color=cyan>[HealthBar]</color> [{bar}] {notification.Current}/{notification.Max}");
        }
    }
}
