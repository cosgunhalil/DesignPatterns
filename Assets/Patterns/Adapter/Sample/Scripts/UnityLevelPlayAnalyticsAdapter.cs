using System;
using System.Collections.Generic;
using DesignPatterns.Adapter.Sample.ThirdParty.UnityLevelPlay;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// ADAPTER for Unity LevelPlay: translates LevelPlay's instance events into
    /// normalized <see cref="AdAnalyticsEvent"/>s.
    ///
    /// LevelPlay specifics this adapter absorbs so nothing else has to know them:
    /// - The reward arrives as its own event (OnAdRewarded), unlike GMA's
    ///   Show-callback and structurally like an event stream.
    /// - OnAdClosed and OnAdRewarded can arrive in EITHER order. This adapter
    ///   never decides cancellation from a close alone — it maps close→Closed
    ///   and reward→Completed, and the base class only resolves Cancelled at
    ///   explicit finalization (closed AND no reward callback ever seen).
    /// - Revenue is a double; normalized to decimal.
    ///
    /// Analytics-only: this class never loads or shows ads.
    /// </summary>
    public sealed class UnityLevelPlayAnalyticsAdapter : AdAnalyticsAdapterBase
    {
        private readonly List<Action> _unsubscribers = new();

        public UnityLevelPlayAnalyticsAdapter(
            IAnalyticsCollector collector,
            IAdAnalyticsContextProvider contextProvider,
            Func<DateTime> utcNow = null)
            : base(AdProvider.UnityLevelPlay, collector, contextProvider, utcNow)
        {
        }

        /// <summary>Subscribe to a rewarded ad instance's lifecycle events.</summary>
        public void TrackRewardedAd(LevelPlayRewardedAd ad)
        {
            var adUnitId = ad.AdUnitId;

            void OnLoaded(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.Loaded, providerEventId: info?.AdId);
            void OnLoadFailed(LevelPlayAdError error) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, null), AdLifecycleEvent.LoadFailed,
                    errorCode: NormalizeErrorCode(error), errorMessage: error?.ErrorMessage);
            void OnDisplayed(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.Displayed, providerEventId: info?.AdId);
            void OnDisplayFailed(LevelPlayAdInfo info, LevelPlayAdError error) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.DisplayFailed,
                    providerEventId: info?.AdId, errorCode: NormalizeErrorCode(error), errorMessage: error?.ErrorMessage);
            void OnClicked(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.Clicked, providerEventId: info?.AdId);
            void OnClosed(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.Closed, providerEventId: info?.AdId);
            void OnRewarded(LevelPlayAdInfo info, LevelPlayReward reward) =>
                Emit(Interaction(adUnitId, AdFormat.Rewarded, info), AdLifecycleEvent.Completed,
                    providerEventId: info?.AdId,
                    rewardType: reward?.Name,
                    rewardAmount: reward?.Amount,
                    rewardEligible: true,
                    rewardGranted: true);
            void OnRevenue(LevelPlayAdInfo info) =>
                EmitRevenue(Interaction(adUnitId, AdFormat.Rewarded, info), info);

            ad.OnAdLoaded += OnLoaded;
            ad.OnAdLoadFailed += OnLoadFailed;
            ad.OnAdDisplayed += OnDisplayed;
            ad.OnAdDisplayFailed += OnDisplayFailed;
            ad.OnAdClicked += OnClicked;
            ad.OnAdClosed += OnClosed;
            ad.OnAdRewarded += OnRewarded;
            ad.OnAdInfoChanged += OnRevenue; // LevelPlay surfaces finalized revenue via info-changed

            _unsubscribers.Add(() =>
            {
                ad.OnAdLoaded -= OnLoaded;
                ad.OnAdLoadFailed -= OnLoadFailed;
                ad.OnAdDisplayed -= OnDisplayed;
                ad.OnAdDisplayFailed -= OnDisplayFailed;
                ad.OnAdClicked -= OnClicked;
                ad.OnAdClosed -= OnClosed;
                ad.OnAdRewarded -= OnRewarded;
                ad.OnAdInfoChanged -= OnRevenue;
            });
        }

        /// <summary>Subscribe to an interstitial ad instance. Reward fields stay null for this format.</summary>
        public void TrackInterstitialAd(LevelPlayInterstitialAd ad)
        {
            var adUnitId = ad.AdUnitId;

            void OnLoaded(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, info), AdLifecycleEvent.Loaded, providerEventId: info?.AdId);
            void OnLoadFailed(LevelPlayAdError error) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, null), AdLifecycleEvent.LoadFailed,
                    errorCode: NormalizeErrorCode(error), errorMessage: error?.ErrorMessage);
            void OnDisplayed(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, info), AdLifecycleEvent.Displayed, providerEventId: info?.AdId);
            void OnDisplayFailed(LevelPlayAdInfo info, LevelPlayAdError error) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, info), AdLifecycleEvent.DisplayFailed,
                    providerEventId: info?.AdId, errorCode: NormalizeErrorCode(error), errorMessage: error?.ErrorMessage);
            void OnClicked(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, info), AdLifecycleEvent.Clicked, providerEventId: info?.AdId);
            void OnClosed(LevelPlayAdInfo info) =>
                Emit(Interaction(adUnitId, AdFormat.Interstitial, info), AdLifecycleEvent.Closed, providerEventId: info?.AdId);
            void OnRevenue(LevelPlayAdInfo info) =>
                EmitRevenue(Interaction(adUnitId, AdFormat.Interstitial, info), info);

            ad.OnAdLoaded += OnLoaded;
            ad.OnAdLoadFailed += OnLoadFailed;
            ad.OnAdDisplayed += OnDisplayed;
            ad.OnAdDisplayFailed += OnDisplayFailed;
            ad.OnAdClicked += OnClicked;
            ad.OnAdClosed += OnClosed;
            ad.OnAdInfoChanged += OnRevenue;

            _unsubscribers.Add(() =>
            {
                ad.OnAdLoaded -= OnLoaded;
                ad.OnAdLoadFailed -= OnLoadFailed;
                ad.OnAdDisplayed -= OnDisplayed;
                ad.OnAdDisplayFailed -= OnDisplayFailed;
                ad.OnAdClicked -= OnClicked;
                ad.OnAdClosed -= OnClosed;
                ad.OnAdInfoChanged -= OnRevenue;
            });
        }

        protected override void Unsubscribe()
        {
            foreach (var unsubscribe in _unsubscribers)
            {
                unsubscribe();
            }

            _unsubscribers.Clear();
        }

        private AdInteraction Interaction(string adUnitId, AdFormat format, LevelPlayAdInfo info) =>
            GetOrCreateInteraction(adUnitId, format, info?.PlacementName);

        private void EmitRevenue(AdInteraction interaction, LevelPlayAdInfo info)
        {
            if (info == null || info.Revenue <= 0)
            {
                return;
            }

            Emit(interaction, AdLifecycleEvent.RevenuePaid,
                providerEventId: info.AdId,
                revenue: (decimal)info.Revenue,
                currencyCode: "USD");
        }

        private static string NormalizeErrorCode(LevelPlayAdError error) =>
            error == null ? null : $"levelplay:{error.ErrorCode}";
    }
}
