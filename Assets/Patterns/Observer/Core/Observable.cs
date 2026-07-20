using System;
using System.Collections.Generic;

namespace DesignPatterns.Observer
{
    /// <summary>
    /// A value that notifies subscribers whenever it changes — a practical,
    /// reusable flavor of Observer (aka a reactive/bindable property). Bind a
    /// health bar, an audio cue, and an achievement to the same value without
    /// any of them knowing about each other. Only a real change notifies, so
    /// setting the same value again is a no-op.
    /// </summary>
    /// <typeparam name="T">The wrapped value type.</typeparam>
    public sealed class Observable<T>
    {
        private readonly List<Action<T>> _subscribers = new();
        private readonly IEqualityComparer<T> _comparer;
        private T _value;

        public Observable(T initialValue = default, IEqualityComparer<T> comparer = null)
        {
            _value = initialValue;
            _comparer = comparer ?? EqualityComparer<T>.Default;
        }

        public T Value
        {
            get => _value;
            set => SetValue(value);
        }

        /// <summary>
        /// Subscribe to changes. When <paramref name="notifyImmediately"/> is true
        /// the current value is pushed once on subscribe — handy for binding UI
        /// that must render the initial state. Disposing the return unsubscribes.
        /// </summary>
        public IDisposable Subscribe(Action<T> onChanged, bool notifyImmediately = false)
        {
            if (onChanged == null)
            {
                throw new ArgumentNullException(nameof(onChanged));
            }

            _subscribers.Add(onChanged);

            if (notifyImmediately)
            {
                onChanged(_value);
            }

            return new Subscription(this, onChanged);
        }

        private void SetValue(T newValue)
        {
            if (_comparer.Equals(_value, newValue))
            {
                return; // no change — no notification
            }

            _value = newValue;

            foreach (var subscriber in _subscribers.ToArray()) // snapshot: safe to unsubscribe mid-notify
            {
                subscriber(newValue);
            }
        }

        private sealed class Subscription : IDisposable
        {
            private Observable<T> _owner;
            private readonly Action<T> _onChanged;

            public Subscription(Observable<T> owner, Action<T> onChanged)
            {
                _owner = owner;
                _onChanged = onChanged;
            }

            public void Dispose()
            {
                _owner?._subscribers.Remove(_onChanged);
                _owner = null;
            }
        }
    }
}
