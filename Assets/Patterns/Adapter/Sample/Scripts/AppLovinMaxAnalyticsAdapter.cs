using System;
using DesignPatterns.Adapter.Sample.ThirdParty.AppLovinMax;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// ADAPTER for AppLovin MAX: translates MAX's static, ad-unit-keyed events
    /// into normalized <see cref="AdAnalyticsEvent"/>s.
    ///
    /// MAX specifics this adapter absorbs so nothing else has to know them:
    /// - Callbacks are STATIC events (MaxSdkCallbacks.Rewarded/...), so
    ///   disposal MUST detach every handler or the adapter leaks forever.
    /// - Revenue is a plain double in USD where -1 means "unavailable" — the
    ///   adapter converts -1 to null instead of reporting negative revenue.
    /// - The reward is a struct (Label, int Amount) on its own event.
    /// - "Hidden" is MAX's name for closed.
    /// - MAX fires no separate impression callback for full-screen ads, so this
    ///   adapter never fabricates ImpressionRecorded.
    ///
    /// Analytics-only: this class never loads or shows ads.
    /// </summary>
    public sealed class AppLovinMaxAnalyticsAdapter : AdAnalyticsAdapterBase
    {
        public AppLovinMaxAnalyticsAdapter(
            IAnalyticsCollector collector,
            IAdAnalyticsContextProvider contextProvider,
            Func<DateTime> utcNow = null)
            : base(AdProvider.AppLovinMax, collector, contextProvider, utcNow)
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedClicked;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedRevenuePaid;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedHidden;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedReceivedReward;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClicked;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaid;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHidden;
        }

        protected override void Unsubscribe()
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedLoaded;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent -= OnRewardedLoadFailed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedDisplayed;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent -= OnRewardedDisplayFailed;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent -= OnRewardedClicked;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnRewardedRevenuePaid;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent -= OnRewardedHidden;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedReceivedReward;

            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent -= OnInterstitialLoadFailed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent -= OnInterstitialDisplayed;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent -= OnInterstitialDisplayFailed;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent -= OnInterstitialClicked;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnInterstitialRevenuePaid;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent -= OnInterstitialHidden;
        }

        // ---- Rewarded ----

        private void OnRewardedLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.Loaded,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnRewardedLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, null), AdLifecycleEvent.LoadFailed,
                errorCode: NormalizeErrorCode(errorInfo),
                errorMessage: errorInfo?.Message);

        private void OnRewardedDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.Displayed,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnRewardedDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.DisplayFailed,
                providerEventId: adInfo?.CreativeIdentifier,
                errorCode: NormalizeErrorCode(errorInfo),
                errorMessage: errorInfo?.Message);

        private void OnRewardedClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.Clicked,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnRewardedRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.RevenuePaid,
                providerEventId: adInfo?.CreativeIdentifier,
                revenue: NormalizeRevenue(adInfo),
                currencyCode: adInfo == null ? null : "USD"); // MAX reports impression revenue in USD

        private void OnRewardedHidden(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.Closed,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnRewardedReceivedReward(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Rewarded, adInfo), AdLifecycleEvent.Completed,
                providerEventId: adInfo?.CreativeIdentifier,
                rewardType: reward.Label,
                rewardAmount: reward.Amount,
                rewardEligible: true,
                rewardGranted: true);

        // ---- Interstitial (no reward events for this format) ----

        private void OnInterstitialLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.Loaded,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnInterstitialLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, null), AdLifecycleEvent.LoadFailed,
                errorCode: NormalizeErrorCode(errorInfo),
                errorMessage: errorInfo?.Message);

        private void OnInterstitialDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.Displayed,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnInterstitialDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.DisplayFailed,
                providerEventId: adInfo?.CreativeIdentifier,
                errorCode: NormalizeErrorCode(errorInfo),
                errorMessage: errorInfo?.Message);

        private void OnInterstitialClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.Clicked,
                providerEventId: adInfo?.CreativeIdentifier);

        private void OnInterstitialRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.RevenuePaid,
                providerEventId: adInfo?.CreativeIdentifier,
                revenue: NormalizeRevenue(adInfo),
                currencyCode: adInfo == null ? null : "USD");

        private void OnInterstitialHidden(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
            Emit(Interaction(adUnitId, AdFormat.Interstitial, adInfo), AdLifecycleEvent.Closed,
                providerEventId: adInfo?.CreativeIdentifier);

        // ---- Translation helpers ----

        private AdInteraction Interaction(string adUnitId, AdFormat format, MaxSdkBase.AdInfo adInfo) =>
            GetOrCreateInteraction(adUnitId, format, adInfo?.Placement);

        /// <summary>MAX uses -1 for "revenue unavailable"; normalize that to null, never a negative value.</summary>
        private static decimal? NormalizeRevenue(MaxSdkBase.AdInfo adInfo) =>
            adInfo == null || adInfo.Revenue < 0 ? (decimal?)null : (decimal)adInfo.Revenue;

        private static string NormalizeErrorCode(MaxSdkBase.ErrorInfo errorInfo) =>
            errorInfo == null ? null : $"max:{errorInfo.Code}:{(int)errorInfo.Code}";
    }
}
