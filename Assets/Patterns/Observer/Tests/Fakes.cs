using System.Collections.Generic;

namespace DesignPatterns.Observer.Tests
{
    /// <summary>Observer that records everything it receives, for assertions.</summary>
    internal sealed class RecordingObserver<TEvent> : IObserver<TEvent>
    {
        public List<TEvent> Received { get; } = new();
        public int Count => Received.Count;

        public void Receive(TEvent notification) => Received.Add(notification);
    }

    /// <summary>Minimal concrete subject exposing a public raise, for testing the base directly.</summary>
    internal sealed class TestSubject<TEvent> : Subject<TEvent>
    {
        public void Raise(TEvent notification) => Notify(notification);
    }

    /// <summary>
    /// Observer that runs an action when it receives — used to exercise
    /// subscribing/unsubscribing from inside a notification.
    /// </summary>
    internal sealed class ReentrantObserver<TEvent> : IObserver<TEvent>
    {
        private readonly System.Action<TEvent> _onReceive;
        public int Count { get; private set; }

        public ReentrantObserver(System.Action<TEvent> onReceive)
        {
            _onReceive = onReceive;
        }

        public void Receive(TEvent notification)
        {
            Count++;
            _onReceive(notification);
        }
    }
}
