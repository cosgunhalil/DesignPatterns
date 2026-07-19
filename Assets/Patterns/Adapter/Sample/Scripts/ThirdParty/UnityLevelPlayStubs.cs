// ============================================================================
// EDUCATIONAL SDK STUBS — NOT the actual Unity LevelPlay plugin.
//
// These types are a REPRESENTATIVE model of the current Unity LevelPlay
// "ad-unit" API (LevelPlayRewardedAd / LevelPlayInterstitialAd and their
// instance events), chosen to teach the Adapter pattern. Unlike the Google
// Mobile Ads and AppLovin MAX stubs in this sample, the exact member names and
// values here are illustrative rather than verified line-by-line against the
// vendor docs — the point is the adapter architecture, not vendor fidelity.
// Do not treat these as the real SDK. Methods prefixed "Simulate" are test
// affordances that do not exist in the real plugin.
//
// The one behaviour that MUST be modeled correctly, because the adapter's
// design depends on it: for a rewarded ad, OnAdClosed and OnAdRewarded may
// arrive in EITHER order. A close arriving first does not mean the user
// cancelled — the reward callback can still follow.
// ============================================================================

using System;

namespace DesignPatterns.Adapter.Sample.ThirdParty.UnityLevelPlay
{
    /// <summary>Representative model of LevelPlayAdInfo.</summary>
    public sealed class LevelPlayAdInfo
    {
        public string AdId { get; init; }
        public string AdUnitId { get; init; }
        public string AdFormat { get; init; }
        public string PlacementName { get; init; }
        public string AdNetwork { get; init; }
        public string InstanceName { get; init; }
        public string Country { get; init; }
        public double Revenue { get; init; }   // representative: USD per impression
        public string Precision { get; init; }
    }

    /// <summary>Representative model of LevelPlayReward.</summary>
    public sealed class LevelPlayReward
    {
        public string Name { get; init; }
        public int Amount { get; init; }
    }

    /// <summary>Representative model of LevelPlayAdError.</summary>
    public sealed class LevelPlayAdError
    {
        public int ErrorCode { get; init; }
        public string ErrorMessage { get; init; }
    }

    /// <summary>
    /// Representative model of LevelPlayRewardedAd: an INSTANCE you construct
    /// with an ad unit id, exposing lifecycle events (contrast with MAX's
    /// static events and GMA's Show-callback reward). The reward is delivered
    /// as its own event here, not as a Show parameter.
    /// </summary>
    public sealed class LevelPlayRewardedAd
    {
        public string AdUnitId { get; }

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo, LevelPlayReward> OnAdRewarded;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        public LevelPlayRewardedAd(string adUnitId)
        {
            AdUnitId = adUnitId;
        }

        // ---- Stub-only simulation methods (not part of the real SDK) ----

        public void SimulateAdLoaded(LevelPlayAdInfo adInfo) => OnAdLoaded?.Invoke(adInfo);
        public void SimulateAdLoadFailed(LevelPlayAdError error) => OnAdLoadFailed?.Invoke(error);
        public void SimulateAdDisplayed(LevelPlayAdInfo adInfo) => OnAdDisplayed?.Invoke(adInfo);
        public void SimulateAdDisplayFailed(LevelPlayAdInfo adInfo, LevelPlayAdError error) => OnAdDisplayFailed?.Invoke(adInfo, error);
        public void SimulateAdClicked(LevelPlayAdInfo adInfo) => OnAdClicked?.Invoke(adInfo);
        public void SimulateAdClosed(LevelPlayAdInfo adInfo) => OnAdClosed?.Invoke(adInfo);
        public void SimulateAdRewarded(LevelPlayAdInfo adInfo, LevelPlayReward reward) => OnAdRewarded?.Invoke(adInfo, reward);
        public void SimulateAdInfoChanged(LevelPlayAdInfo adInfo) => OnAdInfoChanged?.Invoke(adInfo);
    }

    /// <summary>Representative model of LevelPlayInterstitialAd (no reward events).</summary>
    public sealed class LevelPlayInterstitialAd
    {
        public string AdUnitId { get; }

        public event Action<LevelPlayAdInfo> OnAdLoaded;
        public event Action<LevelPlayAdError> OnAdLoadFailed;
        public event Action<LevelPlayAdInfo> OnAdDisplayed;
        public event Action<LevelPlayAdInfo, LevelPlayAdError> OnAdDisplayFailed;
        public event Action<LevelPlayAdInfo> OnAdClicked;
        public event Action<LevelPlayAdInfo> OnAdClosed;
        public event Action<LevelPlayAdInfo> OnAdInfoChanged;

        public LevelPlayInterstitialAd(string adUnitId)
        {
            AdUnitId = adUnitId;
        }

        // ---- Stub-only simulation methods (not part of the real SDK) ----

        public void SimulateAdLoaded(LevelPlayAdInfo adInfo) => OnAdLoaded?.Invoke(adInfo);
        public void SimulateAdLoadFailed(LevelPlayAdError error) => OnAdLoadFailed?.Invoke(error);
        public void SimulateAdDisplayed(LevelPlayAdInfo adInfo) => OnAdDisplayed?.Invoke(adInfo);
        public void SimulateAdDisplayFailed(LevelPlayAdInfo adInfo, LevelPlayAdError error) => OnAdDisplayFailed?.Invoke(adInfo, error);
        public void SimulateAdClicked(LevelPlayAdInfo adInfo) => OnAdClicked?.Invoke(adInfo);
        public void SimulateAdClosed(LevelPlayAdInfo adInfo) => OnAdClosed?.Invoke(adInfo);
        public void SimulateAdInfoChanged(LevelPlayAdInfo adInfo) => OnAdInfoChanged?.Invoke(adInfo);
    }
}
