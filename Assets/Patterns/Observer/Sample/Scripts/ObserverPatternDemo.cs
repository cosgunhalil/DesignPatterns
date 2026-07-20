using System;
using UnityEngine;

namespace DesignPatterns.Observer.Sample
{
    /// <summary>
    /// Entry point. Press Play — it wires several independent observers to one
    /// HealthSystem and runs a damage/heal sequence. Notice: the health system
    /// never references any observer, one observer unsubscribes mid-run via its
    /// disposable token, and the death watcher removes itself during a
    /// notification without breaking the broadcast.
    /// </summary>
    public sealed class ObserverPatternDemo : MonoBehaviour
    {
        private void Start()
        {
            var health = new HealthSystem(100);

            // Class observers subscribed through the IObserver overload.
            health.Subscribe(new HealthBarView());
            health.Subscribe(new LowHealthWarning(0.3f));
            health.Subscribe(new DeathWatcher(health));

            // A lambda observer via the Action overload — no class needed.
            health.Subscribe(change =>
            {
                if (change.IsDead)
                {
                    Debug.Log("<color=grey>[Lambda]</color> fade screen to black.");
                }
            });

            // A subscription we intend to cancel partway through.
            IDisposable combatLog = health.Subscribe(new CombatLog());

            Debug.Log("<b>-- fight begins (100/100) --</b>");
            health.TakeDamage(30);   // 70
            health.TakeDamage(45);   // 25 -> low-health warning fires
            health.Heal(15);         // 40 -> recovers above threshold

            Debug.Log("<b>-- combat log unsubscribes --</b>");
            combatLog.Dispose();     // its token removes it; no more CombatLog lines

            health.TakeDamage(80);   // 0 -> death watcher fires and self-unsubscribes
            health.TakeDamage(10);   // already dead: clamped, no notification at all

            Debug.Log("<b>-- done --</b>");
        }
    }
}
