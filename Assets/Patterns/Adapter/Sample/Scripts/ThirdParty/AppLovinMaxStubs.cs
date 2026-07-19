// ============================================================================
// EDUCATIONAL SDK STUBS — NOT the actual AppLovin MAX plugin.
//
// These types mirror the public API surface of the official AppLovin MAX
// Unity plugin 8.6.4 (event names, delegate signatures and data shapes
// verified against the plugin source MaxSdkCallbacks.cs / MaxSdkBase.cs and
// https://support.applovin.com/en/max/unity/ad-formats/rewarded-ads), so the
// adapter stays structurally compatible with the real SDK. Methods prefixed
// "Simulate" are test affordances that do not exist in the real plugin.
//
// Real-SDK quirks preserved on purpose:
// - Callbacks are STATIC events on nested classes of MaxSdkCallbacks (which is
//   why unsubscribing on dispose matters so much).
// - AdInfo.Revenue is a plain double in USD; -1 means "revenue unavailable".
// - Reward is a struct with an int Amount and a string Label.
// - The Unity plugin supports Rewarded/Interstitial/Banner/AppOpen but NOT
//   native ads; this stub models the two formats the sample exercises.
// ============================================================================

using System;

namespace DesignPatterns.Adapter.Sample.ThirdParty.AppLovinMax
{
    /// <summary>Mirrors MaxSdkBase: the nested data types the callbacks carry.</summary>
    public abstract class MaxSdkBase
    {
        public sealed class AdInfo
        {
            public string AdUnitIdentifier { get; init; }
            public string AdFormat { get; init; }
            public string NetworkName { get; init; }
            public string NetworkPlacement { get; init; }
            public string Placement { get; init; }
            public string CreativeIdentifier { get; init; }
            public double Revenue { get; init; } = -1; // -1 = unavailable, like the real SDK
            public string RevenuePrecision { get; init; }
            public string DspName { get; init; }
            public long LatencyMillis { get; init; }
        }

        public struct Reward
        {
            public string Label;
            public int Amount;

            public bool IsValid() => !string.IsNullOrEmpty(Label) && Amount > 0;

            public override string ToString() => $"Reward: {Amount} {Label}";
        }

        public enum ErrorCode
        {
            Unspecified = -1,
            NoFill = 204,
            AdLoadFailed = -5001,
            AdDisplayFailed = -4205,
            NetworkError = -1000,
            NetworkTimeout = -1001,
            NoNetwork = -1009,
            FullscreenAdAlreadyShowing = -23,
            FullscreenAdNotReady = -24
        }

        public sealed class ErrorInfo
        {
            public ErrorCode Code { get; init; }
            public string Message { get; init; }
            public int MediatedNetworkErrorCode { get; init; }
            public string MediatedNetworkErrorMessage { get; init; }
        }
    }

    /// <summary>Mirrors MaxSdk : MaxSdkBase so both `MaxSdk.AdInfo` and `MaxSdkBase.AdInfo` spellings compile.</summary>
    public sealed class MaxSdk : MaxSdkBase
    {
    }

    /// <summary>
    /// Mirrors MaxSdkCallbacks: static events on nested static classes,
    /// exactly like the real plugin registers them.
    /// </summary>
    public static class MaxSdkCallbacks
    {
        public static class Rewarded
        {
            public static event Action<string, MaxSdkBase.AdInfo> OnAdLoadedEvent;
            public static event Action<string, MaxSdkBase.ErrorInfo> OnAdLoadFailedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdDisplayedEvent;
            public static event Action<string, MaxSdkBase.ErrorInfo, MaxSdkBase.AdInfo> OnAdDisplayFailedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdClickedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdRevenuePaidEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdHiddenEvent;
            public static event Action<string, MaxSdkBase.Reward, MaxSdkBase.AdInfo> OnAdReceivedRewardEvent;

            // ---- Stub-only simulation methods (not part of the real SDK) ----

            public static void SimulateAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdLoadedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
                OnAdLoadFailedEvent?.Invoke(adUnitId, errorInfo);

            public static void SimulateAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdDisplayedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
                OnAdDisplayFailedEvent?.Invoke(adUnitId, errorInfo, adInfo);

            public static void SimulateAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdClickedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdRevenuePaidEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdHiddenEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdReceivedReward(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo) =>
                OnAdReceivedRewardEvent?.Invoke(adUnitId, reward, adInfo);
        }

        public static class Interstitial
        {
            public static event Action<string, MaxSdkBase.AdInfo> OnAdLoadedEvent;
            public static event Action<string, MaxSdkBase.ErrorInfo> OnAdLoadFailedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdDisplayedEvent;
            public static event Action<string, MaxSdkBase.ErrorInfo, MaxSdkBase.AdInfo> OnAdDisplayFailedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdClickedEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdRevenuePaidEvent;
            public static event Action<string, MaxSdkBase.AdInfo> OnAdHiddenEvent;

            // ---- Stub-only simulation methods (not part of the real SDK) ----

            public static void SimulateAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdLoadedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdLoadFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo) =>
                OnAdLoadFailedEvent?.Invoke(adUnitId, errorInfo);

            public static void SimulateAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdDisplayedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdDisplayFailed(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo) =>
                OnAdDisplayFailedEvent?.Invoke(adUnitId, errorInfo, adInfo);

            public static void SimulateAdClicked(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdClickedEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdRevenuePaid(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdRevenuePaidEvent?.Invoke(adUnitId, adInfo);

            public static void SimulateAdHidden(string adUnitId, MaxSdkBase.AdInfo adInfo) =>
                OnAdHiddenEvent?.Invoke(adUnitId, adInfo);
        }
    }
}
