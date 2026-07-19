using UnityEngine;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// Supplies device/app fields from Unity APIs and game fields from state
    /// the game owns. This is the ONLY place Unity statics like SystemInfo and
    /// Application are read — adapters receive the result via the interface and
    /// stay engine-free and testable.
    /// </summary>
    public sealed class UnityAdAnalyticsContextProvider : IAdAnalyticsContextProvider
    {
        private readonly string _userId;
        private readonly string _sessionId;

        // Game state the demo mutates as the player "navigates".
        public string GameMode { get; set; }
        public string LevelId { get; set; }
        public string ScreenName { get; set; }
        public string ExperimentId { get; set; }
        public string ConsentStatus { get; set; }
        public string CountryCode { get; set; }

        public UnityAdAnalyticsContextProvider(string userId, string sessionId)
        {
            _userId = userId;
            _sessionId = sessionId;
        }

        public AdAnalyticsContext GetContext()
        {
            return new AdAnalyticsContext
            {
                UserId = _userId,
                SessionId = _sessionId,
                AppVersion = Application.version,
                Platform = Application.platform.ToString(),
                OperatingSystem = SystemInfo.operatingSystem,
                DeviceModel = SystemInfo.deviceModel,
                CountryCode = CountryCode,
                GameMode = GameMode,
                LevelId = LevelId,
                ScreenName = ScreenName,
                ExperimentId = ExperimentId,
                ConsentStatus = ConsentStatus
            };
        }
    }
}
