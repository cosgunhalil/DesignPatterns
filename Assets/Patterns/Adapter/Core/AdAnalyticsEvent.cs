using System;

namespace DesignPatterns.Adapter
{
    /// <summary>
    /// The single normalized analytics event every provider adapter produces.
    /// Immutable (init-only). Nullable fields are intentional:
    ///
    /// - Revenue/CurrencyCode: only present on RevenuePaid (and only when the
    ///   provider reports revenue for the impression).
    /// - Reward* fields: only meaningful for rewarded ads. For every other
    ///   format they stay null rather than pretending "no reward" is a fact
    ///   the provider stated.
    /// - RewardEligible/RewardGranted: set from the provider's reward callback —
    ///   the provider says the user EARNED the reward. The adapter never grants
    ///   inventory or currency.
    /// - RewardDeliverySucceeded: always null at the adapter boundary. Only the
    ///   game economy or backend knows whether the reward actually reached the
    ///   player; the adapter must not claim it did.
    /// - Error fields: only present on LoadFailed/DisplayFailed.
    /// - ProviderEventId: only when the provider exposes a usable identifier
    ///   (e.g. a response/creative id); correlation never depends on it.
    /// </summary>
    public sealed class AdAnalyticsEvent
    {
        public string EventId { get; init; }
        public string InteractionId { get; init; }

        public string UserId { get; init; }
        public string SessionId { get; init; }

        public AdProvider Provider { get; init; }
        public string ProviderEventId { get; init; }

        public string PlacementId { get; init; }
        public string AdUnitId { get; init; }

        public AdFormat AdFormat { get; init; }
        public AdLifecycleEvent EventType { get; init; }

        public DateTime OccurredAtUtc { get; init; }

        public decimal? Revenue { get; init; }
        public string CurrencyCode { get; init; }

        public string RewardType { get; init; }
        public decimal? RewardAmount { get; init; }

        public bool? RewardEligible { get; init; }
        public bool? RewardGranted { get; init; }
        public bool? RewardDeliverySucceeded { get; init; }
        public string RewardTransactionId { get; init; }

        public string ErrorCode { get; init; }
        public string ErrorMessage { get; init; }

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
}
