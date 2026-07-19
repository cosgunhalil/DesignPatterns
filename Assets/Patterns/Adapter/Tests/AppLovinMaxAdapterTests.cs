using DesignPatterns.Adapter.Sample;
using DesignPatterns.Adapter.Sample.ThirdParty.AppLovinMax;
using NUnit.Framework;

namespace DesignPatterns.Adapter.Tests
{
    public class AppLovinMaxAdapterTests
    {
        private const string AdUnit = "max-rewarded-unit";

        private ListAnalyticsCollector _collector;
        private AppLovinMaxAnalyticsAdapter _adapter;

        [SetUp]
        public void SetUp()
        {
            _collector = new ListAnalyticsCollector();
            _adapter = new AppLovinMaxAnalyticsAdapter(_collector, new FixedContextProvider(), TestClock.UtcNow);
        }

        [TearDown]
        public void TearDown() => _adapter.Dispose();

        private static MaxSdkBase.AdInfo AdInfo(double revenue = -1) => new()
        {
            AdUnitIdentifier = AdUnit,
            AdFormat = "REWARDED",
            NetworkName = "TestNetwork",
            Placement = "shop_bonus",
            CreativeIdentifier = "creative-77",
            Revenue = revenue,
            RevenuePrecision = "exact"
        };

        [Test]
        public void DuplicateRewardCallback_ProducesExactlyOneCompletedEvent()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded, "shop_bonus");
            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdDisplayed(AdUnit, AdInfo());

            var reward = new MaxSdkBase.Reward { Label = "gems", Amount = 10 };
            MaxSdkCallbacks.Rewarded.SimulateAdReceivedReward(AdUnit, reward, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdReceivedReward(AdUnit, reward, AdInfo());

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Completed));
            Assert.AreEqual(1, _adapter.SuppressedDuplicateCount);
        }

        [Test]
        public void Revenue_MinusOne_NormalizesToNull()
        {
            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdRevenuePaid(AdUnit, AdInfo(revenue: -1));

            var revenuePaid = _collector.Single(AdLifecycleEvent.RevenuePaid);
            Assert.IsNull(revenuePaid.Revenue, "MAX revenue of -1 means unavailable, not negative revenue");
        }

        [Test]
        public void Revenue_Positive_ConvertsToDecimalUsd()
        {
            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdRevenuePaid(AdUnit, AdInfo(revenue: 0.0042));

            var revenuePaid = _collector.Single(AdLifecycleEvent.RevenuePaid);
            Assert.AreEqual(0.0042m, revenuePaid.Revenue);
            Assert.AreEqual("USD", revenuePaid.CurrencyCode);
        }

        [Test]
        public void Hidden_MapsToClosed_AndAloneFinalizesAsCancelled()
        {
            _adapter.RecordRequest(AdUnit, AdFormat.Rewarded);
            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdDisplayed(AdUnit, AdInfo());
            MaxSdkCallbacks.Rewarded.SimulateAdHidden(AdUnit, AdInfo());
            _adapter.FinalizeInteraction(AdUnit);

            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Closed));
            Assert.AreEqual(1, _collector.Count(AdLifecycleEvent.Cancelled));
        }

        [Test]
        public void LoadFailure_MapsErrorCodeAndMessage()
        {
            MaxSdkCallbacks.Rewarded.SimulateAdLoadFailed(AdUnit, new MaxSdkBase.ErrorInfo
            {
                Code = MaxSdkBase.ErrorCode.NoFill,
                Message = "No fill for ad unit."
            });

            var failed = _collector.Single(AdLifecycleEvent.LoadFailed);
            Assert.AreEqual("max:NoFill:204", failed.ErrorCode);
            Assert.AreEqual("No fill for ad unit.", failed.ErrorMessage);
        }

        [Test]
        public void Placement_ComesFromMaxAdInfo()
        {
            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());

            Assert.AreEqual("shop_bonus", _collector.Single(AdLifecycleEvent.Loaded).PlacementId);
        }

        [Test]
        public void Dispose_DetachesFromStaticEvents()
        {
            _adapter.Dispose();

            MaxSdkCallbacks.Rewarded.SimulateAdLoaded(AdUnit, AdInfo());

            Assert.AreEqual(0, _collector.Events.Count, "static MAX events must be released on dispose");
        }
    }
}
