using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>Analytics-ish observer: logs each health change as damage or healing.</summary>
    public sealed class CombatLog : IObserver<HealthChanged>
    {
        public void Receive(HealthChanged notification)
        {
            if (notification.Delta < 0)
            {
                Debug.Log($"<color=orange>[CombatLog]</color> took {-notification.Delta} damage");
            }
            else
            {
                Debug.Log($"<color=green>[CombatLog]</color> healed {notification.Delta}");
            }
        }
    }
}
