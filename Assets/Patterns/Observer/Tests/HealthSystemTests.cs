using System;
using NUnit.Framework;
using DesignPatterns.Observer.Sample;

namespace DesignPatterns.Observer.Tests
{
    public class HealthSystemTests
    {
        [Test]
        public void TakeDamage_ReducesHealthAndNotifiesWithDelta()
        {
            var health = new HealthSystem(100);
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.TakeDamage(30);

            Assert.AreEqual(70, health.Current);
            Assert.AreEqual(1, observer.Count);
            Assert.AreEqual(-30, observer.Received[0].Delta);
            Assert.AreEqual(100, observer.Received[0].Previous);
        }

        [Test]
        public void TakeDamage_ClampsAtZeroAndReportsDeath()
        {
            var health = new HealthSystem(50);
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.TakeDamage(999);

            Assert.AreEqual(0, health.Current);
            Assert.IsTrue(observer.Received[0].IsDead);
        }

        [Test]
        public void Heal_ClampsAtMax()
        {
            var health = new HealthSystem(100);
            health.TakeDamage(20); // 80
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.Heal(999);

            Assert.AreEqual(100, health.Current);
            Assert.AreEqual(20, observer.Received[0].Delta);
        }

        [Test]
        public void NoOpChange_DoesNotNotify()
        {
            var health = new HealthSystem(100); // already full
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.Heal(10); // full → no change

            Assert.AreEqual(0, observer.Count);
        }

        [Test]
        public void DamageWhileDead_DoesNotNotify()
        {
            var health = new HealthSystem(30);
            health.TakeDamage(30); // dead
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.TakeDamage(10); // still 0

            Assert.AreEqual(0, observer.Count);
        }

        [Test]
        public void Fraction_ReflectsCurrentOverMax()
        {
            var health = new HealthSystem(200);
            var observer = new RecordingObserver<HealthChanged>();
            health.Subscribe(observer);

            health.TakeDamage(50); // 150/200

            Assert.AreEqual(0.75f, observer.Received[0].Fraction, 0.0001f);
        }

        [Test]
        public void NegativeInputs_Throw()
        {
            var health = new HealthSystem(100);

            Assert.Throws<ArgumentOutOfRangeException>(() => health.TakeDamage(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => health.Heal(-1));
        }

        [Test]
        public void Constructor_RejectsNonPositiveMax()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new HealthSystem(0));
        }

        [Test]
        public void DeathWatcher_FiresOnceAndSelfUnsubscribes()
        {
            var health = new HealthSystem(40);
            var watcher = new DeathWatcher(health);
            health.Subscribe(watcher);

            health.TakeDamage(40);

            Assert.IsTrue(watcher.HasDied);
            Assert.AreEqual(0, health.ObserverCount, "the watcher should have removed itself during notify");
        }
    }
}
