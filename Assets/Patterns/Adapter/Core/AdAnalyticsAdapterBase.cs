using System;
using System.Collections.Generic;

namespace DesignPatterns.Adapter
{
    /// <summary>
    /// Provider-agnostic bookkeeping shared by every analytics adapter:
    /// interaction correlation, duplicate suppression, rewarded-ad state,
    /// context stamping and synchronous forwarding to the collector.
    ///
    /// Concrete adapters contain ONLY provider translation — subscribing to
    /// provider callbacks and mapping provider data onto Emit calls. There is
    /// deliberately no provider switch anywhere in this class.
    /// </summary>
    public abstract class AdAnalyticsAdapterBase : IDisposable
    {
        private readonly IAnalyticsCollector _collector;
        private readonly IAdAnalyticsContextProvider _contextProvider;
        private readonly Func<DateTime> _utcNow;

        // One "current" interaction per ad unit: providers key callbacks by ad
        // unit id, and a unit hosts one interaction at a time.
        private readonly Dictionary<string, AdInteraction> _interactionsByAdUnit = new();
        private readonly HashSet<string> _seenEventKeys = new();

        private bool _disposed;

        public AdProvider Provider { get; }

        /// <summary>How many exact-duplicate provider callbacks were suppressed (visible for the demo and tests).</summary>
        public int SuppressedDuplicateCount { get; private set; }

        protected AdAnalyticsAdapterBase(
            AdProvider provider,
            IAnalyticsCollector collector,
            IAdAnalyticsContextProvider contextProvider,
            Func<DateTime> utcNow = null)
        {
            Provider = provider;
            _collector = collector ?? throw new ArgumentNullException(nameof(collector));
            _contextProvider = contextProvider ?? throw new ArgumentNullException(nameof(contextProvider));
            _utcNow = utcNow ?? (() => DateTime.UtcNow);
        }

        /// <summary>
        /// Analytics-only: called by the game's ad manager immediately BEFORE it
        /// asks the SDK to load an ad. Creates the local correlation id used for
        /// the rest of the interaction and emits Requested. Does NOT load ads.
        /// </summary>
        public string RecordRequest(string adUnitId, AdFormat format, string placementId = null)
        {
            if (string.IsNullOrEmpty(adUnitId))
            {
                throw new ArgumentException("Ad unit id is required.", nameof(adUnitId));
            }

            var interaction = new AdInteraction(NewId(), adUnitId, format, placementId)
            {
                WasRequested = true
            };
            _interactionsByAdUnit[adUnitId] = interaction;

            Emit(interaction, AdLifecycleEvent.Requested);
            return interaction.InteractionId;
        }

        /// <summary>
        /// Finalizes the current interaction for an ad unit. For rewarded ads
        /// this is where Cancelled is decided: only when the ad was closed and
        /// no reward callback ever arrived. Close arriving before the reward
        /// callback must never be treated as cancellation by itself.
        /// </summary>
        public void FinalizeInteraction(string adUnitId)
        {
            if (!_interactionsByAdUnit.TryGetValue(adUnitId, out var interaction) || interaction.IsFinalized)
            {
                return;
            }

            interaction.IsFinalized = true;

            var isCancelledRewardedAd = interaction.Format == AdFormat.Rewarded
                                        && interaction.WasClosed
                                        && !interaction.RewardCallbackReceived;
            if (isCancelledRewardedAd)
            {
                Emit(interaction, AdLifecycleEvent.Cancelled, allowAfterFinalization: true);
            }
        }

        /// <summary>
        /// Returns the current interaction for an ad unit, creating one when a
        /// provider callback arrives without a preceding RecordRequest (SDKs may
        /// cache-fill or auto-refresh outside the game's request flow).
        /// </summary>
        protected AdInteraction GetOrCreateInteraction(string adUnitId, AdFormat format, string placementId = null)
        {
            if (_interactionsByAdUnit.TryGetValue(adUnitId, out var existing) && !existing.IsFinalized)
            {
                if (placementId != null)
                {
                    existing.PlacementId = placementId;
                }

                return existing;
            }

            var interaction = new AdInteraction(NewId(), adUnitId, format, placementId);
            _interactionsByAdUnit[adUnitId] = interaction;
            return interaction;
        }

        /// <summary>
        /// Builds the normalized event, suppresses exact duplicates, updates the
        /// interaction's rewarded state and forwards synchronously to the
        /// collector. Duplicate suppression here is a client-side courtesy — it
        /// does not replace backend idempotency.
        /// </summary>
        protected void Emit(
            AdInteraction interaction,
            AdLifecycleEvent eventType,
            string providerEventId = null,
            decimal? revenue = null,
            string currencyCode = null,
            string rewardType = null,
            decimal? rewardAmount = null,
            bool? rewardEligible = null,
            bool? rewardGranted = null,
            string errorCode = null,
            string errorMessage = null,
            bool allowAfterFinalization = false)
        {
            if (_disposed || interaction == null)
            {
                return;
            }

            if (interaction.IsFinalized && !allowAfterFinalization)
            {
                return;
            }

            var duplicateKey = $"{Provider}|{interaction.InteractionId}|{providerEventId}|{interaction.AdUnitId}|{eventType}";
            if (!_seenEventKeys.Add(duplicateKey))
            {
                SuppressedDuplicateCount++;
                return;
            }

            switch (eventType)
            {
                case AdLifecycleEvent.Displayed:
                    interaction.WasDisplayed = true;
                    break;
                case AdLifecycleEvent.Completed:
                    interaction.RewardCallbackReceived = true;
                    break;
                case AdLifecycleEvent.Closed:
                    interaction.WasClosed = true;
                    break;
            }

            var context = _contextProvider.GetContext() ?? new AdAnalyticsContext();

            _collector.Collect(new AdAnalyticsEvent
            {
                EventId = NewId(),
                InteractionId = interaction.InteractionId,
                UserId = context.UserId,
                SessionId = context.SessionId,
                Provider = Provider,
                ProviderEventId = providerEventId,
                PlacementId = interaction.PlacementId,
                AdUnitId = interaction.AdUnitId,
                AdFormat = interaction.Format,
                EventType = eventType,
                OccurredAtUtc = _utcNow(),
                Revenue = revenue,
                CurrencyCode = currencyCode,
                RewardType = rewardType,
                RewardAmount = rewardAmount,
                RewardEligible = rewardEligible,
                RewardGranted = rewardGranted,
                RewardDeliverySucceeded = null, // only the game economy/backend may ever set this
                RewardTransactionId = null,
                ErrorCode = errorCode,
                ErrorMessage = errorMessage,
                AppVersion = context.AppVersion,
                Platform = context.Platform,
                OperatingSystem = context.OperatingSystem,
                DeviceModel = context.DeviceModel,
                CountryCode = context.CountryCode,
                GameMode = context.GameMode,
                LevelId = context.LevelId,
                ScreenName = context.ScreenName,
                ExperimentId = context.ExperimentId,
                ConsentStatus = context.ConsentStatus
            });
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            Unsubscribe();
        }

        /// <summary>Detach from all provider callbacks. Called exactly once, from Dispose.</summary>
        protected abstract void Unsubscribe();

        private static string NewId() => Guid.NewGuid().ToString("N");
    }
}
