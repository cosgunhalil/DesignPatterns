using System;
using System.Collections.Generic;
using System.Linq;

namespace DesignPatterns.Adapter.Tests
{
    /// <summary>Test collector: captures every normalized event for assertions.</summary>
    internal sealed class ListAnalyticsCollector : IAnalyticsCollector
    {
        public List<AdAnalyticsEvent> Events { get; } = new();

        public void Collect(AdAnalyticsEvent analyticsEvent) => Events.Add(analyticsEvent);

        public AdAnalyticsEvent Single(AdLifecycleEvent type) => Events.Single(e => e.EventType == type);

        public int Count(AdLifecycleEvent type) => Events.Count(e => e.EventType == type);

        public List<AdLifecycleEvent> Sequence() => Events.Select(e => e.EventType).ToList();
    }

    /// <summary>Deterministic context for tests.</summary>
    internal sealed class FixedContextProvider : IAdAnalyticsContextProvider
    {
        public AdAnalyticsContext Context { get; set; } = new()
        {
            UserId = "user-1",
            SessionId = "session-1",
            AppVersion = "1.2.3",
            Platform = "TestPlatform",
            OperatingSystem = "TestOS",
            DeviceModel = "TestDevice",
            CountryCode = "TR",
            GameMode = "arcade",
            LevelId = "level-7",
            ScreenName = "level_complete",
            ExperimentId = "exp-42",
            ConsentStatus = "granted"
        };

        public AdAnalyticsContext GetContext() => Context;
    }

    /// <summary>Fixed clock so OccurredAtUtc is assertable.</summary>
    internal static class TestClock
    {
        public static readonly DateTime Now = new(2026, 7, 19, 12, 0, 0, DateTimeKind.Utc);
        public static DateTime UtcNow() => Now;
    }
}
