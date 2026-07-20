using System;
using NUnit.Framework;

namespace DesignPatterns.Observer.Tests
{
    public class SubjectTests
    {
        private TestSubject<int> _subject;

        [SetUp]
        public void SetUp()
        {
            _subject = new TestSubject<int>();
        }

        [Test]
        public void Notify_DeliversToAllObservers()
        {
            var a = new RecordingObserver<int>();
            var b = new RecordingObserver<int>();
            _subject.Subscribe(a);
            _subject.Subscribe(b);

            _subject.Raise(42);

            CollectionAssert.AreEqual(new[] { 42 }, a.Received);
            CollectionAssert.AreEqual(new[] { 42 }, b.Received);
        }

        [Test]
        public void Unsubscribe_StopsDelivery()
        {
            var observer = new RecordingObserver<int>();
            _subject.Subscribe(observer);

            _subject.Unsubscribe(observer);
            _subject.Raise(1);

            Assert.AreEqual(0, observer.Count);
        }

        [Test]
        public void DisposingSubscription_Unsubscribes()
        {
            var observer = new RecordingObserver<int>();
            var token = _subject.Subscribe(observer);

            token.Dispose();
            _subject.Raise(1);

            Assert.AreEqual(0, observer.Count);
            Assert.AreEqual(0, _subject.ObserverCount);
        }

        [Test]
        public void DisposingSubscriptionTwice_IsHarmless()
        {
            var token = _subject.Subscribe(new RecordingObserver<int>());

            token.Dispose();
            Assert.DoesNotThrow(() => token.Dispose());
        }

        [Test]
        public void DoubleSubscribe_SameObserver_NotifiedOnce()
        {
            var observer = new RecordingObserver<int>();
            _subject.Subscribe(observer);
            _subject.Subscribe(observer);

            _subject.Raise(7);

            Assert.AreEqual(1, observer.Count);
            Assert.AreEqual(1, _subject.ObserverCount);
        }

        [Test]
        public void ActionOverload_ReceivesNotifications()
        {
            var received = 0;
            _subject.Subscribe(value => received = value);

            _subject.Raise(99);

            Assert.AreEqual(99, received);
        }

        [Test]
        public void Subscribe_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _subject.Subscribe((IObserver<int>)null));
            Assert.Throws<ArgumentNullException>(() => _subject.Subscribe((Action<int>)null));
        }

        [Test]
        public void ObserverUnsubscribingDuringNotify_DoesNotSkipOthers()
        {
            // The legacy bug: an observer detaching mid-notification shifted the
            // live list and the next observer was skipped. Snapshot iteration fixes it.
            var tail = new RecordingObserver<int>();
            ReentrantObserver<int> selfRemover = null;
            selfRemover = new ReentrantObserver<int>(_ => _subject.Unsubscribe(selfRemover));

            _subject.Subscribe(selfRemover);
            _subject.Subscribe(tail);

            _subject.Raise(1);

            Assert.AreEqual(1, tail.Count, "the observer after the self-removing one must still be notified");
            Assert.AreEqual(1, _subject.ObserverCount);
        }

        [Test]
        public void SubscribingDuringNotify_DoesNotReceiveCurrentRound()
        {
            var late = new RecordingObserver<int>();
            _subject.Subscribe(new ReentrantObserver<int>(_ => _subject.Subscribe(late)));

            _subject.Raise(1); // 'late' subscribes during this round
            Assert.AreEqual(0, late.Count, "an observer added mid-broadcast waits for the next one");

            _subject.Raise(2);
            Assert.AreEqual(1, late.Count);
        }
    }
}
