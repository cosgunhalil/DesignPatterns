using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace DesignPatterns.EventQueue.Tests
{
    public class EventQueueTests
    {
        private EventQueue<string> _queue;
        private List<string> _handled;

        [SetUp]
        public void SetUp()
        {
            _queue = new EventQueue<string>();
            _handled = new List<string>();
        }

        [Test]
        public void Enqueue_DoesNotDispatchUntilProcess()
        {
            _queue.Subscribe(_handled.Add);

            _queue.Enqueue("a");

            Assert.AreEqual(1, _queue.PendingCount);
            Assert.IsEmpty(_handled, "nothing should be handled before Process");
        }

        [Test]
        public void Process_DispatchesPendingEventsInFifoOrder()
        {
            _queue.Subscribe(_handled.Add);
            _queue.Enqueue("a");
            _queue.Enqueue("b");
            _queue.Enqueue("c");

            _queue.Process();

            CollectionAssert.AreEqual(new[] { "a", "b", "c" }, _handled);
            Assert.AreEqual(0, _queue.PendingCount);
        }

        [Test]
        public void EventsEnqueuedDuringProcess_WaitForNextProcess()
        {
            // A handler re-posts once; the re-posted event must NOT be handled in the same drain.
            var reposted = false;
            _queue.Subscribe(e =>
            {
                _handled.Add(e);
                if (e == "first" && !reposted)
                {
                    reposted = true;
                    _queue.Enqueue("second");
                }
            });
            _queue.Enqueue("first");

            _queue.Process();
            CollectionAssert.AreEqual(new[] { "first" }, _handled);
            Assert.AreEqual(1, _queue.PendingCount, "the re-posted event waits for the next Process");

            _queue.Process();
            CollectionAssert.AreEqual(new[] { "first", "second" }, _handled);
        }

        [Test]
        public void Process_CalledFromInsideAHandler_Throws()
        {
            _queue.Subscribe(_ => _queue.Process()); // re-entrant call
            _queue.Enqueue("boom");

            Assert.Throws<InvalidOperationException>(() => _queue.Process());
        }

        [Test]
        public void MultipleSubscribers_AllReceiveEachEvent()
        {
            var second = new List<string>();
            _queue.Subscribe(_handled.Add);
            _queue.Subscribe(second.Add);
            _queue.Enqueue("x");

            _queue.Process();

            CollectionAssert.AreEqual(new[] { "x" }, _handled);
            CollectionAssert.AreEqual(new[] { "x" }, second);
        }

        [Test]
        public void DisposingSubscription_StopsDelivery()
        {
            var token = _queue.Subscribe(_handled.Add);
            token.Dispose();

            _queue.Enqueue("x");
            _queue.Process();

            Assert.IsEmpty(_handled);
        }

        [Test]
        public void UnsubscribeDuringDispatch_DoesNotAffectOtherHandlers()
        {
            var other = new List<string>();
            IDisposable token = null;
            token = _queue.Subscribe(_ => token.Dispose()); // removes itself mid-dispatch
            _queue.Subscribe(other.Add);
            _queue.Enqueue("x");

            _queue.Process();

            CollectionAssert.AreEqual(new[] { "x" }, other, "the other handler must still fire");
        }

        [Test]
        public void Subscribe_Null_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => _queue.Subscribe(null));
        }

        [Test]
        public void Clear_DropsPendingWithoutDispatching()
        {
            _queue.Subscribe(_handled.Add);
            _queue.Enqueue("a");
            _queue.Enqueue("b");

            _queue.Clear();
            _queue.Process();

            Assert.AreEqual(0, _queue.PendingCount);
            Assert.IsEmpty(_handled);
        }

        [Test]
        public void Process_WithNoSubscribers_JustDrains()
        {
            _queue.Enqueue("a");

            Assert.DoesNotThrow(() => _queue.Process());
            Assert.AreEqual(0, _queue.PendingCount);
        }
    }
}
