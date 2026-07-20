namespace DesignPatterns.Observer
{
    /// <summary>
    /// Something that reacts to notifications a subject broadcasts. This is the
    /// Gang-of-Four "Observer" role and is intentionally distinct from the BCL's
    /// <c>System.IObserver&lt;T&gt;</c> (which is Rx-style OnNext/OnError/OnCompleted).
    /// Here a single command — <see cref="Receive"/> — is all an observer owes.
    /// </summary>
    /// <typeparam name="TEvent">The notification payload.</typeparam>
    public interface IObserver<in TEvent>
    {
        void Receive(TEvent notification);
    }
}
