using System;
using System.Collections.Generic;

namespace DesignPatterns.EventQueue
{
    /// <summary>
    /// Generic deferred event queue. Events are buffered on <see cref="Enqueue"/>
    /// and dispatched to subscribers on <see cref="Process"/>.
    ///
    /// Two subtleties that make an event queue correct rather than a foot-gun:
    /// 1. <b>Frame-bounded drain.</b> <see cref="Process"/> handles only the events
    ///    that were pending when it started. Anything a handler enqueues waits for
    ///    the next <see cref="Process"/> — so a handler that re-posts an event can't
    ///    spin the loop forever, and work is naturally spread across frames.
    /// 2. <b>No re-entrancy.</b> Calling <see cref="Process"/> from inside a handler
    ///    throws, rather than interleaving two drains.
    /// </summary>
    /// <typeparam name="TEvent">The buffered event payload.</typeparam>
    public sealed class EventQueue<TEvent> : IEventQueue<TEvent>
    {
        private readonly Queue<TEvent> _pending = new();
        private readonly List<Action<TEvent>> _handlers = new();
        private bool _processing;

        public int PendingCount => _pending.Count;
        public int HandlerCount => _handlers.Count;

        public void Enqueue(TEvent notification) => _pending.Enqueue(notification);

        public IDisposable Subscribe(Action<TEvent> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            _handlers.Add(handler);
            return new Subscription(this, handler);
        }

        public void Process()
        {
            if (_processing)
            {
                throw new InvalidOperationException("Process is already running; a handler must not call Process again.");
            }

            _processing = true;
            try
            {
                // Snapshot the count now: events a handler enqueues during this
                // drain land behind this boundary and wait for the next Process.
                var drainCount = _pending.Count;
                for (var i = 0; i < drainCount; i++)
                {
                    Dispatch(_pending.Dequeue());
                }
            }
            finally
            {
                _processing = false;
            }
        }

        public void Clear() => _pending.Clear();

        private void Dispatch(TEvent notification)
        {
            // Snapshot handlers so one can unsubscribe (or subscribe) during dispatch.
            var snapshot = _handlers.ToArray();
            foreach (var handler in snapshot)
            {
                handler(notification);
            }
        }

        private sealed class Subscription : IDisposable
        {
            private EventQueue<TEvent> _queue;
            private readonly Action<TEvent> _handler;

            public Subscription(EventQueue<TEvent> queue, Action<TEvent> handler)
            {
                _queue = queue;
                _handler = handler;
            }

            public void Dispose()
            {
                _queue?._handlers.Remove(_handler);
                _queue = null;
            }
        }
    }
}
