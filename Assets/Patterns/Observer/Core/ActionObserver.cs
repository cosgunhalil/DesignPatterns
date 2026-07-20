using System;

namespace DesignPatterns.Observer
{
    /// <summary>
    /// Adapts a plain delegate into an <see cref="IObserver{TEvent}"/>, so callers
    /// can subscribe a lambda instead of writing a whole observer class for a
    /// one-liner. Used by <see cref="Subject{TEvent}.Subscribe(Action{TEvent})"/>.
    /// </summary>
    internal sealed class ActionObserver<TEvent> : IObserver<TEvent>
    {
        private readonly Action<TEvent> _onNotify;

        public ActionObserver(Action<TEvent> onNotify)
        {
            _onNotify = onNotify;
        }

        public void Receive(TEvent notification) => _onNotify(notification);
    }
}
