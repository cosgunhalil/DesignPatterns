using DesignPatterns.Adapter.Sample;
using DesignPatterns.Adapter.Sample.ThirdParty.UnityLevelPlay;
using NUnit.Framework;

namespace DesignPatterns.Adapter.Tests
{
    public class UnityLevelPlayAdapterTests
    {
        private const string AdUnit = "levelplay-rewarded-unit";

        private ListAnalyticsCollector _collector;
        private UnityLevelPlayAnalyticsAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _collector = new ListAnalyticsCollector();
            _adapter = new UnityLevelPlayAnalyticsAdapter(_collector, new FixedContextProvider(), TestClock.UtcNow);
        }

        [TearDown]
        public void TearDown() => _adapter.Dispose();

        private static LevelPlayAdInfo Info(double revenue = 0) => new()
        {
            AdId = "lp-ad-1",
            AdUnitId = AdUnit,
            AdFormat = "RewardedVideo",
            PlacementName = "continue_run",
            AdNetwork = "ironSource",
            Revenue = revenue,
            Precision = "exact"
        };

        private LevelPlayRewardedAd TrackedRewardedAd()
        {
            var ad = new LevelPlayRewardedAd(AdUnit);
            _adapter.TrackRewardedAd(ad);
            return ad;
        }

        [Test]
        public void CloseBeforeReward_IsNotCancelled_AndFinalizesAsCompleted()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "continue_run");
            var ad = TrackedRewardedAd();

            ad.SimulateAdLoaded(Info());
            ad.SimulateAdDisplayed(Info());
            ad.SimulateAdClosed(Info());  // close arrives FIRST
            ad.SimulateAdRewarded(Info(), new LevelPlayReward { Name = "extra_life", Amount = 1 }); // reward AFTER
            _adapter.FinalizeInteraction(AdUnit);

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Closed));
            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Completed));
            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Cancelled),
                "a reward after close must not be classified as cancellation");
        }

        [Test]
        public void RewardBeforeClose_AlsoCompletesWithoutCancellation()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "continue_run");
            var ad = TrackedRewardedAd();

            ad.SimulateAdLoaded(Info());
            ad.SimulateAdDisplayed(Info());
            ad.SimulateAdRewarded(Info(), new LevelPlayReward { Name = "extra_life", Amount = 1 }); // reward FIRST
            ad.SimulateAdClosed(Info());
            _adapter.FinalizeInteraction(AdUnit);

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Completed));
            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Cancelled));
        }

        [Test]
        public void ClosedWithNoReward_FinalizesAsCancelled()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "continue_run");
            var ad = TrackedRewardedAd();

            ad.SimulateAdLoaded(Info());
            ad.SimulateAdDisplayed(Info());
            ad.SimulateAdClosed(Info());
            _adapter.FinalizeInteraction(AdUnit);

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Cancelled));
            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Completed));
        }

        [Test]
        public void Reward_MapsFieldsAndLeavesDeliveryUnknown()
        {
            var ad = TrackedRewardedAd();

            ad.SimulateAdRewarded(Info(), new LevelPlayReward { Name = "extra_life", Amount = 2 });

            var completed = _collector.Single(AdLifecycleEvent.Completed);
            Assert.AreEqual("extra_life", completed.RewardType);
            Assert.AreEqual(2m, completed.RewardAmount);
            Assert.IsTrue(completed.RewardEligible);
            Assert.IsTrue(completed.RewardGranted);
            Assert.IsNull(completed.RewardDeliverySucceeded);
        }

        [Test]
        public void Revenue_FromInfoChanged_ConvertsToDecimal()
        {
            var ad = TrackedRewardedAd();

            ad.SimulateAdInfoChanged(Info(revenue: 0.0088));

            Assert.AreEqual(0.0088m, _collector.Single(AdLifecycleEvent.RevenuePaid).Revenue);
        }

        [Test]
        public void DisplayFailure_MapsNormalizedError()
        {
            var ad = TrackedRewardedAd();

            ad.SimulateAdDisplayFailed(Info(), new LevelPlayAdError { ErrorCode = 509, ErrorMessage = "No ads to show." });

            var failed = _collector.Single(AdLifecycleEvent.DisplayFailed);
            Assert.AreEqual("levelplay:509", failed.ErrorCode);
            Assert.AreEqual("No ads to show.", failed.ErrorMessage);
        }

        [Test]
        public void Dispose_UnsubscribesFromInstanceEvents()
        {
            var ad = TrackedRewardedAd();
            _adapter.Dispose();

            ad.SimulateAdLoaded(Info());

            Assert.AreEqual(0, _collector.Events.Count);
        }
    }
}
