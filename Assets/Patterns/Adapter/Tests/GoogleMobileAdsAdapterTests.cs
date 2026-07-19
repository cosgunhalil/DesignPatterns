using System.Linq;
using DesignPatterns.Adapter.Sample;
using DesignPatterns.Adapter.Sample.ThirdParty.GoogleMobileAds;
using NUnit.Framework;

namespace DesignPatterns.Adapter.Tests
{
    public class GoogleMobileAdsAdapterTests
    {
        private const string AdUnit = "gma-rewarded-unit";

        private ListAnalyticsCollector _collector;
        private GoogleMobileAdsAnalyticsAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _collector = new ListAnalyticsCollector();
            _adapter = new GoogleMobileAdsAnalyticsAdapter(_collector, new FixedContextProvider(), TestClock.UtcNow);
        }

        [TearDown]
        public void TearDown() => _adapter.Dispose();

        private RewardedAd LoadTrackedRewardedAd()
        {
            RewardedAd loaded = null;
            RewardedAd.Load(AdUnit, new AdRequest(), (ad, error) => loaded = ad);
            _adapter.TrackRewardedAd(AdUnit, loaded);
            return loaded;
        }

        [Test]
        public void HappyPath_EmitsTheFullNormalizedLifecycle()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "level_complete");
            var ad = LoadTrackedRewardedAd();

            ad.SimulateFullScreenContentOpened();
            ad.SimulateImpressionRecorded();
            ad.SimulatePaid(new AdValue { Value = 1_230_000, CurrencyCode = "USD", Precision = AdValue.PrecisionType.Precise });
            _adapter.TrackRewardEarned(AdUnit, new Reward { Type = "coins", Amount = 25 });
            ad.SimulateFullScreenContentClosed();
            _adapter.FinalizeInteraction(AdUnit);

            CollectionAssert.AreEqual(new[]
            {
                AdLifecycleEvent.Requested,
                AdLifecycleEvent.Loaded,
                AdLifecycleEvent.Displayed,
                AdLifecycleEvent.ImpressionRecorded,
                AdLifecycleEvent.RevenuePaid,
                AdLifecycleEvent.Completed,
                AdLifecycleEvent.Closed
            }, _collector.Sequence());

            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Cancelled), "completed ad must not be cancelled");
        }

        [Test]
        public void Revenue_ConvertsMicrosToDecimalUnits()
        {
            var ad = LoadTrackedRewardedAd();

            ad.SimulatePaid(new AdValue { Value = 1_230_000, CurrencyCode = "USD" });

            var revenuePaid = _collector.Single(AdLifecycleEvent.RevenuePaid);
            Assert.AreEqual(1.23m, revenuePaid.Revenue);
            Assert.AreEqual("USD", revenuePaid.CurrencyCode);
        }

        [Test]
        public void RewardEarned_MapsRewardAndClaimsNothingAboutDelivery()
        {
            LoadTrackedRewardedAd();

            _adapter.TrackRewardEarned(AdUnit, new Reward { Type = "coins", Amount = 25.0 });

            var completed = _collector.Single(AdLifecycleEvent.Completed);
            Assert.AreEqual("coins", completed.RewardType);
            Assert.AreEqual(25m, completed.RewardAmount);
            Assert.IsTrue(completed.RewardEligible);
            Assert.IsTrue(completed.RewardGranted);
            Assert.IsNull(completed.RewardDeliverySucceeded, "adapter must not claim the reward was delivered");
            Assert.IsNull(completed.RewardTransactionId);
        }

        [Test]
        public void ClosedWithoutReward_FinalizesAsCancelled()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "level_complete");
            var ad = LoadTrackedRewardedAd();

            ad.SimulateFullScreenContentOpened();
            ad.SimulateFullScreenContentClosed();
            _adapter.FinalizeInteraction(AdUnit);

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Cancelled));
            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Completed));
        }

        [Test]
        public void LoadFailure_MapsNormalizedError()
        {
            RewardedAd.NextLoadError = new LoadAdError(3, "MobileAds", "No fill.");
            RewardedAd.Load(AdUnit, new AdRequest(), (ad, error) =>
            {
                if (error != null)
                {
                    _adapter.TrackLoadFailure(AdUnit, AdFormat.Rewarded, error);
                }
            });

            var failed = _collector.Single(AdLifecycleEvent.LoadFailed);
            Assert.AreEqual("gma:MobileAds:3", failed.ErrorCode);
            Assert.AreEqual("No fill.", failed.ErrorMessage);
        }

        [Test]
        public void Interstitial_LeavesRewardFieldsNull()
        {
            InterstitialAd loaded = null;
            InterstitialAd.Load("gma-interstitial-unit", new AdRequest(), (ad, error) => loaded = ad);
            _adapter.TrackInterstitialAd("gma-interstitial-unit", loaded);

            loaded.SimulateFullScreenContentOpened();
            loaded.SimulateFullScreenContentClosed();
            _adapter.FinalizeInteraction("gma-interstitial-unit");

            Assert.IsTrue(_collector.Events.All(e => e.RewardType == null && e.RewardAmount == null
                                                     && e.RewardEligible == null && e.RewardGranted == null));
            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Cancelled), "interstitial close is not a cancellation");
        }

        [Test]
        public void DuplicateProviderCallback_IsSuppressed()
        {
            var ad = LoadTrackedRewardedAd();

            ad.SimulateFullScreenContentOpened();
            ad.SimulateFullScreenContentOpened();

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Displayed));
            Assert.AreEqual(1, _adapter.SuppressedDuplicateCount);
        }

        [Test]
        public void Dispose_UnsubscribesFromAdEvents()
        {
            var ad = LoadTrackedRewardedAd();
            _adapter.Dispose();

            ad.SimulateFullScreenContentOpened();

            Assert.AreEqual(0, _collector.Count(AdLifecycleEvent.Displayed));
        }

        [Test]
        public void Events_CarryContextAndCorrelation()
        {
            var interactionId = _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "level_complete");
            LoadTrackedRewardedAd();

            Assert.IsTrue(_collector.Events.All(e => e.InteractionId == interactionId));
            Assert.IsTrue(_collector.Events.All(e => e.UserId == "user-1" && e.SessionId == "session-1"));
            Assert.IsTrue(_collector.Events.All(e => e.OccurredAtUtc == TestClock.Now));
            Assert.IsTrue(_collector.Events.All(e => e.Provider == AdProvider.GoogleMobileAds));
            Assert.IsTrue(_collector.Events.All(e => e.PlacementId == "level_complete"));
        }
    }
}
