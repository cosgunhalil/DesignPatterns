using System;

namespace DesignPatterns.Observer
{
    /// <summary>
    /// The thing being observed. Callers subscribe to receive future
    /// notifications; the subject decides when to broadcast them. Subscribing
    /// returns an <see cref="IDisposable"/> token — disposing it unsubscribes,
    /// which is far harder to forget than a matching manual Unsubscribe call.
    /// </summary>
    /// <typeparam name="TEvent">The notification payload.</typeparam>
    public interface ISubject<TEvent>
    {
        IDisposable Subscribe(IObserver<TEvent> observer);

        IDisposable Subscribe(Action<TEvent> onNotify);

        void Unsubscribe(IObserver<TEvent> observer);
    }
}
