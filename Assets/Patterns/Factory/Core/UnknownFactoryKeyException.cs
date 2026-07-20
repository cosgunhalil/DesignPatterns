using System;

namespace DesignPatterns.Factory
{
    /// <summary>
    /// Thrown by a keyed factory's <c>Create</c> when no creator is registered
    /// for the requested key. Use <c>CanCreate</c> or <c>TryCreate</c> to avoid it
    /// when a missing key is an expected, non-exceptional case.
    /// </summary>
    public sealed class UnknownFactoryKeyException : Exception
    {
        public object Key { get; }

        public UnknownFactoryKeyException(object key)
            : base($"No creator is registered for key '{key}'.")
        {
            Key = key;
        }
    }
}
