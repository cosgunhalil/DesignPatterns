namespace DesignPatterns.Adapter
{
    /// <summary>
    /// Ambient game/device context stamped onto every normalized event.
    /// Immutable; supplied via <see cref="IAdAnalyticsContextProvider"/> so
    /// adapters never touch Unity statics (SystemInfo, Application, scenes)
    /// directly — which keeps them engine-free and unit-testable.
    /// </summary>
    public sealed class AdAnalyticsContext
    {
        public string UserId { get; init; }
        public string SessionId { get; init; }
        public string AppVersion { get; init; }
        public string Platform { get; init; }
        public string OperatingSystem { get; init; }
        public string DeviceModel { get; init; }
        public string CountryCode { get; init; }
        public string GameMode { get; init; }
        public string LevelId { get; init; }
        public string ScreenName { get; init; }
        public string ExperimentId { get; init; }
        public string ConsentStatus { get; init; }
    }

    /// <summary>Supplies the current analytics context; injected into every adapter.</summary>
    public interface IAdAnalyticsContextProvider
    {
        AdAnalyticsContext GetContext();
    }
}
