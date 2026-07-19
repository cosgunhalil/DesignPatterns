namespace DesignPatterns.Adapter
{
    /// <summary>
    /// The TARGET interface of this Adapter example: the one contract the game
    /// speaks. Adapters translate provider callbacks into normalized events and
    /// forward them here synchronously. Implementations must contain no
    /// provider-specific logic — if a provider name shows up in a collector,
    /// the adaptation has leaked.
    /// </summary>
    public interface IAnalyticsCollector
    {
        void Collect(AdAnalyticsEvent analyticsEvent);
    }
}
