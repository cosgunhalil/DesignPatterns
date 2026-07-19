using DesignPatterns.Adapter.Sample.ThirdParty;
using UnityEngine;
using Gma = DesignPatterns.Adapter.Sample.ThirdParty.GoogleMobileAds;
using Max = DesignPatterns.Adapter.Sample.ThirdParty.AppLovinMax;
using Lp = DesignPatterns.Adapter.Sample.ThirdParty.UnityLevelPlay;

namespace DesignPatterns.Adapter.Sample
{
    /// <summary>
    /// Drives the six teaching scenarios through the three adapters and prints
    /// the normalized results. Press Play — everything runs once in Start and
    /// logs to the Console. Notice that all three providers, with completely
    /// different SDK shapes, produce identically-structured analytics events.
    ///
    /// This demo plays the role of the game's ad manager: it owns ad loading
    /// and showing (here, stub simulation calls) and calls RecordRequest before
    /// requesting. The adapters only translate callbacks into analytics.
    /// </summary>
    public sealed class AdapterPatternDemo : MonoBehaviour
    {
        private void Start()
        {
            var collector = new ConsoleAnalyticsCollector();
            var context = new UnityAdAnalyticsContextProvider(userId: "player-42", sessionId: "session-abc")
            {
                CountryCode = "TR",
                GameMode = "campaign",
                LevelId = "level-12",
                ScreenName = "level_complete",
                ExperimentId = "reward-ui-B",
                ConsentStatus = "granted"
            };

            RunGoogleRewardedCompleted(collector, context);
            RunAppLovinRewardedWithDuplicateReward(collector, context);
            RunLevelPlayRewardedCloseBeforeReward(collector, context);
            RunLevelPlayRewardedCancelled(collector, context);
            RunAppLovinInterstitialNoReward(collector, context);
            RunGoogleRewardedLoadFailure(collector, context);

            Debug.Log("<b>Adapter demo complete — every line above is the same AdAnalyticsEvent shape.</b>");
        }

        // Scenario 1 — Google Mobile Ads rewarded, completed with reward.
        private static void RunGoogleRewardedCompleted(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 1: Google Mobile Ads rewarded (completed) ──</b>");
            using var adapter = new GoogleMobileAdsAnalyticsAdapter(collector, context);
            const string adUnit = "ca-app-pub/rewarded";

            adapter.RecordRequest(adUnit, AdFormat.Rewarded, "level_complete");

            Gma.RewardedAd loaded = null;
            Gma.RewardedAd.Load(adUnit, new Gma.AdRequest(), (ad, error) => loaded = ad);
            adapter.TrackRewardedAd(adUnit, loaded);

            loaded.SimulateFullScreenContentOpened();
            loaded.SimulateImpressionRecorded();
            loaded.SimulatePaid(new Gma.AdValue { Value = 2_500_000, CurrencyCode = "USD", Precision = Gma.AdValue.PrecisionType.Precise });
            adapter.TrackRewardEarned(adUnit, new Gma.Reward { Type = "coins", Amount = 50 });
            loaded.SimulateFullScreenContentClosed();
            adapter.FinalizeInteraction(adUnit);
        }

        // Scenario 2 — AppLovin MAX rewarded, reward callback fires twice (one suppressed).
        private static void RunAppLovinRewardedWithDuplicateReward(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 2: AppLovin MAX rewarded (duplicate reward suppressed) ──</b>");
            using var adapter = new AppLovinMaxAnalyticsAdapter(collector, context);
            const string adUnit = "max-rewarded";

            Max.MaxSdkBase.AdInfo Info() => new()
            {
                AdUnitIdentifier = adUnit, AdFormat = "REWARDED", NetworkName = "AppLovin",
                Placement = "shop_double_coins", CreativeIdentifier = "cr-1001", Revenue = 0.0123, RevenuePrecision = "exact"
            };

            adapter.RecordRequest(adUnit, AdFormat.Rewarded, "shop_double_coins");
            Max.MaxSdkCallbacks.Rewarded.SimulateAdLoaded(adUnit, Info());
            Max.MaxSdkCallbacks.Rewarded.SimulateAdDisplayed(adUnit, Info());
            Max.MaxSdkCallbacks.Rewarded.SimulateAdClicked(adUnit, Info());
            Max.MaxSdkCallbacks.Rewarded.SimulateAdRevenuePaid(adUnit, Info());

            var reward = new Max.MaxSdkBase.Reward { Label = "gems", Amount = 5 };
            Max.MaxSdkCallbacks.Rewarded.SimulateAdReceivedReward(adUnit, reward, Info());
            Max.MaxSdkCallbacks.Rewarded.SimulateAdReceivedReward(adUnit, reward, Info()); // duplicate — suppressed

            Max.MaxSdkCallbacks.Rewarded.SimulateAdHidden(adUnit, Info());
            adapter.FinalizeInteraction(adUnit);

            Debug.Log($"MAX suppressed {adapter.SuppressedDuplicateCount} duplicate callback(s).");
        }

        // Scenario 3 — LevelPlay rewarded, close arrives BEFORE reward; must NOT be cancelled.
        private static void RunLevelPlayRewardedCloseBeforeReward(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 3: LevelPlay rewarded (close before reward, still completed) ──</b>");
            using var adapter = new UnityLevelPlayAnalyticsAdapter(collector, context);
            const string adUnit = "levelplay-rewarded";

            Lp.LevelPlayAdInfo Info() => new()
            {
                AdId = "lp-ad-9", AdUnitId = adUnit, AdFormat = "RewardedVideo", PlacementName = "continue_run",
                AdNetwork = "ironSource", InstanceName = "Bidding", Country = "TR", Revenue = 0.0088, Precision = "exact"
            };

            var ad = new Lp.LevelPlayRewardedAd(adUnit);
            adapter.RecordRequest(adUnit, AdFormat.Rewarded, "continue_run");
            adapter.TrackRewardedAd(ad);

            ad.SimulateAdLoaded(Info());
            ad.SimulateAdDisplayed(Info());
            ad.SimulateAdInfoChanged(Info());          // revenue finalized
            ad.SimulateAdClosed(Info());               // close FIRST
            ad.SimulateAdRewarded(Info(), new Lp.LevelPlayReward { Name = "extra_life", Amount = 1 }); // reward AFTER
            adapter.FinalizeInteraction(adUnit);        // completed, not cancelled
        }

        // Scenario 4 — LevelPlay rewarded, closed with no reward → cancelled.
        private static void RunLevelPlayRewardedCancelled(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 4: LevelPlay rewarded (cancelled — closed, no reward) ──</b>");
            using var adapter = new UnityLevelPlayAnalyticsAdapter(collector, context);
            const string adUnit = "levelplay-rewarded";

            Lp.LevelPlayAdInfo Info() => new()
            {
                AdId = "lp-ad-10", AdUnitId = adUnit, AdFormat = "RewardedVideo", PlacementName = "continue_run",
                AdNetwork = "ironSource", InstanceName = "Bidding", Country = "TR", Revenue = 0, Precision = "exact"
            };

            var ad = new Lp.LevelPlayRewardedAd(adUnit);
            adapter.RecordRequest(adUnit, AdFormat.Rewarded, "continue_run");
            adapter.TrackRewardedAd(ad);

            ad.SimulateAdLoaded(Info());
            ad.SimulateAdDisplayed(Info());
            ad.SimulateAdClosed(Info());        // user dismissed early
            adapter.FinalizeInteraction(adUnit); // no reward ever seen → cancelled
        }

        // Scenario 5 — non-rewarded format: MAX interstitial, reward fields stay null.
        private static void RunAppLovinInterstitialNoReward(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 5: AppLovin MAX interstitial (no reward fields) ──</b>");
            using var adapter = new AppLovinMaxAnalyticsAdapter(collector, context);
            const string adUnit = "max-interstitial";

            Max.MaxSdkBase.AdInfo Info() => new()
            {
                AdUnitIdentifier = adUnit, AdFormat = "INTER", NetworkName = "AppLovin",
                Placement = "between_levels", CreativeIdentifier = "cr-2002", Revenue = 0.0071, RevenuePrecision = "estimated"
            };

            adapter.RecordRequest(adUnit, AdFormat.Interstitial, "between_levels");
            Max.MaxSdkCallbacks.Interstitial.SimulateAdLoaded(adUnit, Info());
            Max.MaxSdkCallbacks.Interstitial.SimulateAdDisplayed(adUnit, Info());
            Max.MaxSdkCallbacks.Interstitial.SimulateAdRevenuePaid(adUnit, Info());
            Max.MaxSdkCallbacks.Interstitial.SimulateAdHidden(adUnit, Info());
            adapter.FinalizeInteraction(adUnit); // interstitial close is NOT a cancellation
        }

        // Scenario 6 — load failure: GMA rewarded, error normalized.
        private static void RunGoogleRewardedLoadFailure(IAnalyticsCollector collector, IAdAnalyticsContextProvider context)
        {
            Debug.Log("<b>── Scenario 6: Google Mobile Ads rewarded (load failure) ──</b>");
            using var adapter = new GoogleMobileAdsAnalyticsAdapter(collector, context);
            const string adUnit = "ca-app-pub/rewarded";

            adapter.RecordRequest(adUnit, AdFormat.Rewarded, "level_complete");

            Gma.RewardedAd.NextLoadError = new Gma.LoadAdError(3, "com.google.android.gms.ads", "No ad to show.");
            Gma.RewardedAd.Load(adUnit, new Gma.AdRequest(), (ad, error) =>
            {
                if (error != null)
                {
                    adapter.TrackLoadFailure(adUnit, AdFormat.Rewarded, error);
                }
            });
        }
    }
}
