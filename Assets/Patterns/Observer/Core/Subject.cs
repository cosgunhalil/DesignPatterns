using System;
using System.Collections.Generic;

namespace DesignPatterns.Observer
{
    /// <summary>
    /// Reusable base for any observable subject. Manages the observer list,
    /// hands out disposable subscriptions, and broadcasts via <see cref="Notify"/>.
    /// Subclasses decide WHEN to notify (they call the protected <see cref="Notify"/>);
    /// the outside world can only subscribe. Generic over the event payload, so
    /// one base serves every subject in a codebase.
    /// </summary>
    /// <typeparam name="TEvent">The notification payload broadcast to observers.</typeparam>
    public abstract class Subject<TEvent> : ISubject<TEvent>
    {
        private readonly List<IObserver<TEvent>> _observers = new();

        /// <summary>Number of current observers. Exposed mainly for tests and teaching.</summary>
        public int ObserverCount => _observers.Count;

        public IDisposable Subscribe(IObserver<TEvent> observer)
        {
            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            // Ignore a repeat subscribe so an observer can't be notified twice.
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }

            return new Subscription(this, observer);
        }

        public IDisposable Subscribe(Action<TEvent> onNotify)
        {
            if (onNotify == null)
            {
                throw new ArgumentNullException(nameof(onNotify));
            }

            return Subscribe(new ActionObserver<TEvent>(onNotify));
        }

        public void Unsubscribe(IObserver<TEvent> observer) => _observers.Remove(observer);

        /// <summary>
        /// Broadcast to every current observer. Iterates a snapshot so an observer
        /// may unsubscribe (or subscribe) from inside <see cref="IObserver{TEvent}.Receive"/>
        /// without corrupting the loop — the exact place the legacy sample's
        /// index-based loop silently skipped observers.
        /// </summary>
        protected void Notify(TEvent notification)
        {
            var snapshot = _observers.ToArray();
            foreach (var observer in snapshot)
            {
                observer.Receive(notification);
            }
        }

        private sealed class Subscription : IDisposable
        {
            private Subject<TEvent> _subject;
            private readonly IObserver<TEvent> _observer;

            public Subscription(Subject<TEvent> subject, IObserver<TEvent> observer)
            {
                _subject = subject;
                _observer = observer;
            }

            public void Dispose()
            {
                // Idempotent: disposing twice is harmless.
                _subject?.Unsubscribe(_observer);
                _subject = null;
            }
        }
    }
}
