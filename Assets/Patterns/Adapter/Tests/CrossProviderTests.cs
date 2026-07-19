using System.Collections.Generic;
using System.Linq;
using DesignPatterns.Adapter.Sample;
using DesignPatterns.Adapter.Sample.ThirdParty.AppLovinMax;
using DesignPatterns.Adapter.Sample.ThirdParty.UnityLevelPlay;
using NUnit.Framework;
using Gma = DesignPatterns.Adapter.Sample.ThirdParty.GoogleMobileAds;

namespace DesignPatterns.Adapter.Tests
{
    /// <summary>
    /// The payoff of the pattern: events from three unrelated SDKs arrive at the
    /// collector with one identical shape, and the collector never branches on
    /// provider.
    /// </summary>
    public class CrossProviderTests
    {
        [Test]
        public void AllThreeProviders_ProduceTheSameNormalizedShape()
        {
            var collector = new ListAnalyticsCollector();
            var context = new FixedContextProvider();

            // Google
            using (var gma = new GoogleMobileAdsAnalyticsAdapter(collector, context, TestClock.UtcNow))
            {
                Gma.RewardedAd ad = null;
                Gma.RewardedAd.Load("g", new Gma.AdRequest(), (a, e) => ad = a);
                gma.RecordRequest("g", AdFormat.Rewarded, "p");
                gma.TrackRewardedAd("g", ad);
                gma.TrackRewardEarned("g", new Gma.Reward { Type = "coins", Amount = 10 });
            }

            // AppLovin
            using (var max = new AppLovinMaxAnalyticsAdapter(collector, context, TestClock.UtcNow))
            {
                var info = new MaxSdkBase.AdInfo { AdUnitIdentifier = "m", Placement = "p", Revenue = 0.01, CreativeIdentifier = "c" };
                max.RecordRequest("m", AdFormat.Rewarded, "p");
                MaxSdkCallbacks.Rewarded.SimulateAdReceivedReward("m", new MaxSdkBase.Reward { Label = "gems", Amount = 3 }, info);
            }

            // LevelPlay
            using (var lp = new UnityLevelPlayAnalyticsAdapter(collector, context, TestClock.UtcNow))
            {
                var ad = new LevelPlayRewardedAd("l");
                lp.RecordRequest("l", AdFormat.Rewarded, "p");
                lp.TrackRewardedAd(ad);
                ad.SimulateAdRewarded(new LevelPlayAdInfo { AdId = "x", AdUnitId = "l", PlacementName = "p" },
                    new LevelPlayReward { Name = "life", Amount = 1 });
            }

            var completedByProvider = collector.Events
                .Where(e => e.EventType == AdLifecycleEvent.Completed)
                .ToDictionary(e => e.Provider);

            CollectionAssert.AreEquivalent(
                new[] { AdProvider.GoogleMobileAds, AdProvider.AppLovinMax, AdProvider.UnityLevelPlay },
                completedByProvider.Keys);

            // Every provider's Completed event carries the same normalized facts.
            foreach (var completed in completedByProvider.Values)
            {
                Assert.IsTrue(completed.RewardEligible);
                Assert.IsTrue(completed.RewardGranted);
                Assert.IsNull(completed.RewardDeliverySucceeded);
                Assert.IsNotNull(completed.RewardType);
                Assert.IsNotNull(completed.InteractionId);
                Assert.AreEqual("user-1", completed.UserId);
                Assert.AreEqual(AdFormat.Rewarded, completed.AdFormat);
            }
        }

        [Test]
        public void EveryEvent_CarriesAStableInteractionIdPerProvider()
        {
            var collector = new ListAnalyticsCollector();
            var context = new FixedContextProvider();

            using var lp = new UnityLevelPlayAnalyticsAdapter(collector, context, TestClock.UtcNow);
            var ad = new LevelPlayRewardedAd("l");
            var interactionId = lp.RecordRequest("l", AdFormat.Rewarded, "p");
            lp.TrackRewardedAd(ad);

            var info = new LevelPlayAdInfo { AdId = "x", AdUnitId = "l", PlacementName = "p" };
            ad.SimulateAdLoaded(info);
            ad.SimulateAdDisplayed(info);

            var ids = collector.Events.Select(e => e.InteractionId).Distinct().ToList();
            Assert.AreEqual(1, ids.Count);
            Assert.AreEqual(interactionId, ids[0]);
        }
    }
}
