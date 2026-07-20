using System.Collections.Generic;
using NUnit.Framework;

namespace DesignPatterns.Observer.Tests
{
    public class ObservableTests
    {
        [Test]
        public void SettingValue_NotifiesSubscribers()
        {
            var observable = new Observable<int>(0);
            var seen = new List<int>();
            observable.Subscribe(seen.Add);

            observable.Value = 5;

            CollectionAssert.AreEqual(new[] { 5 }, seen);
            Assert.AreEqual(5, observable.Value);
        }

        [Test]
        public void SettingSameValue_DoesNotNotify()
        {
            var observable = new Observable<int>(5);
            var notifications = 0;
            observable.Subscribe(_ => notifications++);

            observable.Value = 5; // unchanged

            Assert.AreEqual(0, notifications);
        }

        [Test]
        public void Subscribe_NotifyImmediately_PushesCurrentValue()
        {
            var observable = new Observable<string>("start");
            string seen = null;

            observable.Subscribe(v => seen = v, notifyImmediately: true);

            Assert.AreEqual("start", seen);
        }

        [Test]
        public void Dispose_StopsNotifications()
        {
            var observable = new Observable<int>(0);
            var count = 0;
            var token = observable.Subscribe(_ => count++);

            observable.Value = 1;
            token.Dispose();
            observable.Value = 2;

            Assert.AreEqual(1, count);
        }

        [Test]
        public void MultipleSubscribers_AllNotified()
        {
            var observable = new Observable<int>(0);
            int a = 0, b = 0;
            observable.Subscribe(v => a = v);
            observable.Subscribe(v => b = v);

            observable.Value = 9;

            Assert.AreEqual(9, a);
            Assert.AreEqual(9, b);
        }

        [Test]
        public void CustomComparer_ControlsWhatCountsAsAChange()
        {
            // Case-insensitive: "HI" equals "hi", so no notification.
            var observable = new Observable<string>("hi", System.StringComparer.OrdinalIgnoreCase);
            var notifications = 0;
            observable.Subscribe(_ => notifications++);

            observable.Value = "HI";
            Assert.AreEqual(0, notifications);

            observable.Value = "bye";
            Assert.AreEqual(1, notifications);
        }
    }
}
