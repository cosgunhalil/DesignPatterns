using System;

namespace DesignPatterns.EventQueue
{
    /// <summary>
    /// A queue that decouples <em>when</em> an event is sent from <em>when</em> it
    /// is handled. Producers <see cref="Enqueue"/> events at any time; nothing runs
    /// until someone calls <see cref="Process"/> (typically once per frame). That
    /// time-decoupling is what sets this apart from the Observer pattern, whose
    /// notifications fire synchronously, inline with the sender.
    /// </summary>
    /// <typeparam name="TEvent">The buffered event payload.</typeparam>
    public interface IEventQueue<TEvent>
    {
        /// <summary>Number of events buffered and waiting for the next <see cref="Process"/>.</summary>
        int PendingCount { get; }

        /// <summary>Buffer an event for later processing. Returns immediately; nothing is dispatched here.</summary>
        void Enqueue(TEvent notification);

        /// <summary>Register a handler for processed events. Dispose the return value to unsubscribe.</summary>
        IDisposable Subscribe(Action<TEvent> handler);

        /// <summary>
        /// Dispatch the events buffered before this call to every subscriber, in
        /// FIFO order. Events enqueued <em>during</em> processing are held for the
        /// next call — never handled in the same drain.
        /// </summary>
        void Process();

        /// <summary>Discard all pending events without dispatching them.</summary>
        void Clear();
    }
}
