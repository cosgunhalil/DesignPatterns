using System;
using System.Collections.Generic;
using DesignPatterns.Adapter.Sample.ThirdParty.GoogleMobileAds;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// ADAPTER for Google Mobile Ads: translates GMA's per-instance events and
    /// data shapes into normalized <see cref="AdAnalyticsEvent"/>s.
    ///
    /// GMA specifics this adapter absorbs so nothing else has to know them:
    /// - Ads are one object per load; events are instance events, so the ad
    ///   manager hands each loaded ad to Track*(…) and we subscribe there.
    /// - Revenue arrives in micro-units (long); we convert to decimal units.
    /// - The reward is NOT an event — GMA passes it to the callback given to
    ///   Show(). The ad manager forwards it via TrackRewardEarned.
    /// - Reward.Amount is a double; normalized to decimal.
    ///
    /// Analytics-only: this class never loads or shows ads.
    /// </summary>
    public sealed class GoogleMobileAdsAnalyticsAdapter : AdAnalyticsAdapterBase
    {
        private readonly List<Action> _unsubscribers = new();

        public GoogleMobileAdsAnalyticsAdapter(
            IAnalyticsCollector collector,
            IAdAnalyticsContextProvider contextProvider,
            Func<DateTime> utcNow = null)
            : base(AdProvider.GoogleMobileAds, collector, contextProvider, utcNow)
        {
        }

        /// <summary>Call after RewardedAd.Load succeeds; subscribes to the instance's lifecycle events.</summary>
        public void TrackRewardedAd(string adUnitId, RewardedAd ad)
        {
            var interaction = GetOrCreateInteraction(adUnitId, AdFormat.Rewarded);
            var responseId = ad.GetResponseInfo()?.GetResponseId();

            Emit(interaction, AdLifecycleEvent.Loaded, providerEventId: responseId);

            void OnOpened() => Emit(interaction, AdLifecycleEvent.Displayed, providerEventId: responseId);
            void OnImpression() => Emit(interaction, AdLifecycleEvent.ImpressionRecorded, providerEventId: responseId);
            void OnClicked() => Emit(interaction, AdLifecycleEvent.Clicked, providerEventId: responseId);
            void OnPaid(AdValue adValue) => EmitRevenue(interaction, responseId, adValue);
            void OnClosed() => Emit(interaction, AdLifecycleEvent.Closed, providerEventId: responseId);
            void OnFailed(AdError error) => EmitDisplayFailed(interaction, responseId, error);

            ad.OnAdFullScreenContentOpened += OnOpened;
            ad.OnAdImpressionRecorded += OnImpression;
            ad.OnAdClicked += OnClicked;
            ad.OnAdPaid += OnPaid;
            ad.OnAdFullScreenContentClosed += OnClosed;
            ad.OnAdFullScreenContentFailed += OnFailed;

            _unsubscribers.Add(() =>
            {
                ad.OnAdFullScreenContentOpened -= OnOpened;
                ad.OnAdImpressionRecorded -= OnImpression;
                ad.OnAdClicked -= OnClicked;
                ad.OnAdPaid -= OnPaid;
                ad.OnAdFullScreenContentClosed -= OnClosed;
                ad.OnAdFullScreenContentFailed -= OnFailed;
            });
        }

        /// <summary>Call after InterstitialAd.Load succeeds. Reward fields stay null for this format.</summary>
        public void TrackInterstitialAd(string adUnitId, InterstitialAd ad)
        {
            var interaction = GetOrCreateInteraction(adUnitId, AdFormat.Interstitial);
            var responseId = ad.GetResponseInfo()?.GetResponseId();

            Emit(interaction, AdLifecycleEvent.Loaded, providerEventId: responseId);

            void OnOpened() => Emit(interaction, AdLifecycleEvent.Displayed, providerEventId: responseId);
            void OnImpression() => Emit(interaction, AdLifecycleEvent.ImpressionRecorded, providerEventId: responseId);
            void OnClicked() => Emit(interaction, AdLifecycleEvent.Clicked, providerEventId: responseId);
            void OnPaid(AdValue adValue) => EmitRevenue(interaction, responseId, adValue);
            void OnClosed() => Emit(interaction, AdLifecycleEvent.Closed, providerEventId: responseId);
            void OnFailed(AdError error) => EmitDisplayFailed(interaction, responseId, error);

            ad.OnAdFullScreenContentOpened += OnOpened;
            ad.OnAdImpressionRecorded += OnImpression;
            ad.OnAdClicked += OnClicked;
            ad.OnAdPaid += OnPaid;
            ad.OnAdFullScreenContentClosed += OnClosed;
            ad.OnAdFullScreenContentFailed += OnFailed;

            _unsubscribers.Add(() =>
            {
                ad.OnAdFullScreenContentOpened -= OnOpened;
                ad.OnAdImpressionRecorded -= OnImpression;
                ad.OnAdClicked -= OnClicked;
                ad.OnAdPaid -= OnPaid;
                ad.OnAdFullScreenContentClosed -= OnClosed;
                ad.OnAdFullScreenContentFailed -= OnFailed;
            });
        }

        /// <summary>Call from the Load callback when GMA reports a load failure.</summary>
        public void TrackLoadFailure(string adUnitId, AdFormat format, LoadAdError error)
        {
            var interaction = GetOrCreateInteraction(adUnitId, format);
            Emit(interaction, AdLifecycleEvent.LoadFailed,
                errorCode: NormalizeErrorCode(error),
                errorMessage: error?.GetMessage());
        }

        /// <summary>
        /// Call from the reward callback the ad manager passed to RewardedAd.Show.
        /// The provider says the user EARNED the reward — hence Eligible/Granted
        /// are true — but delivery is unknown at this boundary, so
        /// RewardDeliverySucceeded stays null (set by the base for every event).
        /// </summary>
        public void TrackRewardEarned(string adUnitId, Reward reward)
        {
            var interaction = GetOrCreateInteraction(adUnitId, AdFormat.Rewarded);
            Emit(interaction, AdLifecycleEvent.Completed,
                rewardType: reward.Type,
                rewardAmount: (decimal)reward.Amount,
                rewardEligible: true,
                rewardGranted: true);
        }

        protected override void Unsubscribe()
        {
            foreach (var unsubscribe in _unsubscribers)
            {
                unsubscribe();
            }

            _unsubscribers.Clear();
        }

        private void EmitRevenue(AdInteraction interaction, string responseId, AdValue adValue)
        {
            // GMA reports micros: 1,000,000 micro-units = 1 currency unit.
            Emit(interaction, AdLifecycleEvent.RevenuePaid,
                providerEventId: responseId,
                revenue: adValue.Value / 1_000_000m,
                currencyCode: adValue.CurrencyCode);
        }

        private void EmitDisplayFailed(AdInteraction interaction, string responseId, AdError error)
        {
            Emit(interaction, AdLifecycleEvent.DisplayFailed,
                providerEventId: responseId,
                errorCode: NormalizeErrorCode(error),
                errorMessage: error?.GetMessage());
        }

        private static string NormalizeErrorCode(AdError error) =>
            error == null ? null : $"gma:{error.GetDomain()}:{error.GetCode()}";
    }
}
