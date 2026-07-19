// ============================================================================
// EDUCATIONAL SDK STUBS — NOT the actual Google Mobile Ads plugin.
//
// These types mirror the public API surface of the official Google Mobile Ads
// Unity plugin v11.3.0 (class names, event names, delegate signatures and data
// shapes verified against https://developers.google.com/admob/unity/reference)
// so the adapters remain structurally compatible with the real SDK. Methods
// prefixed "Simulate" are test affordances that do not exist in the real
// plugin — they let the demo and tests fire callbacks without real ads.
// ============================================================================

using System;

namespace DesignPatterns.Adapter.Sample.ThirdParty.GoogleMobileAds
{
    /// <summary>Mirrors GoogleMobileAds.Api.AdRequest (empty stand-in).</summary>
    public sealed class AdRequest
    {
    }

    /// <summary>Mirrors GoogleMobileAds.Api.AdValue: revenue in micro-units (1,000,000 micros = 1 unit).</summary>
    public sealed class AdValue
    {
        public enum PrecisionType
        {
            Unknown = 0,
            Estimated = 1,
            PublisherProvided = 2,
            Precise = 3
        }

        public long Value;
        public string CurrencyCode;
        public PrecisionType Precision;
    }

    /// <summary>Mirrors GoogleMobileAds.Api.Reward. Note Amount is a double in the real SDK.</summary>
    public sealed class Reward
    {
        public string Type;
        public double Amount;
    }

    /// <summary>Mirrors GoogleMobileAds.Api.AdError (method-based accessors, like the real class).</summary>
    public class AdError
    {
        private readonly int _code;
        private readonly string _domain;
        private readonly string _message;

        public AdError(int code, string domain, string message)
        {
            _code = code;
            _domain = domain;
            _message = message;
        }

        public int GetCode() => _code;
        public string GetDomain() => _domain;
        public string GetMessage() => _message;
    }

    /// <summary>Mirrors GoogleMobileAds.Api.LoadAdError.</summary>
    public sealed class LoadAdError : AdError
    {
        public LoadAdError(int code, string domain, string message) : base(code, domain, message)
        {
        }
    }

    /// <summary>Mirrors GoogleMobileAds.Api.ResponseInfo (only the member the sample needs).</summary>
    public sealed class ResponseInfo
    {
        private readonly string _responseId;

        public ResponseInfo(string responseId)
        {
            _responseId = responseId;
        }

        public string GetResponseId() => _responseId;
    }

    /// <summary>
    /// Mirrors GoogleMobileAds.Api.RewardedAd: static Load, Show with the
    /// reward callback, and the six instance events of the real class.
    /// </summary>
    public sealed class RewardedAd
    {
        /// <summary>Stub-only hook: set to make the next Load call report a failure once.</summary>
        public static LoadAdError NextLoadError;

        public event Action<AdValue> OnAdPaid;
        public event Action OnAdImpressionRecorded;
        public event Action OnAdClicked;
        public event Action OnAdFullScreenContentOpened;
        public event Action OnAdFullScreenContentClosed;
        public event Action<AdError> OnAdFullScreenContentFailed;

        private readonly ResponseInfo _responseInfo;
        private Action<Reward> _userRewardEarnedCallback;

        private RewardedAd(string responseId)
        {
            _responseInfo = new ResponseInfo(responseId);
        }

        public static void Load(string adUnitId, AdRequest request, Action<RewardedAd, LoadAdError> adLoadCallback)
        {
            if (NextLoadError != null)
            {
                var error = NextLoadError;
                NextLoadError = null;
                adLoadCallback(null, error);
                return;
            }

            adLoadCallback(new RewardedAd($"gma-response-{Guid.NewGuid():N}"), null);
        }

        public void Show(Action<Reward> userRewardEarnedCallback)
        {
            _userRewardEarnedCallback = userRewardEarnedCallback;
        }

        public ResponseInfo GetResponseInfo() => _responseInfo;

        // ---- Stub-only simulation methods (not part of the real SDK) ----

        public void SimulateFullScreenContentOpened() => OnAdFullScreenContentOpened?.Invoke();
        public void SimulateImpressionRecorded() => OnAdImpressionRecorded?.Invoke();
        public void SimulateClicked() => OnAdClicked?.Invoke();
        public void SimulatePaid(AdValue adValue) => OnAdPaid?.Invoke(adValue);
        public void SimulateUserEarnedReward(Reward reward) => _userRewardEarnedCallback?.Invoke(reward);
        public void SimulateFullScreenContentClosed() => OnAdFullScreenContentClosed?.Invoke();
        public void SimulateFullScreenContentFailed(AdError error) => OnAdFullScreenContentFailed?.Invoke(error);
    }

    /// <summary>
    /// Mirrors GoogleMobileAds.Api.InterstitialAd: same six events as
    /// RewardedAd, but Show takes no reward callback.
    /// </summary>
    public sealed class InterstitialAd
    {
        public event Action<AdValue> OnAdPaid;
        public event Action OnAdImpressionRecorded;
        public event Action OnAdClicked;
        public event Action OnAdFullScreenContentOpened;
        public event Action OnAdFullScreenContentClosed;
        public event Action<AdError> OnAdFullScreenContentFailed;

        private readonly ResponseInfo _responseInfo;

        private InterstitialAd(string responseId)
        {
            _responseInfo = new ResponseInfo(responseId);
        }

        public static void Load(string adUnitId, AdRequest request, Action<InterstitialAd, LoadAdError> adLoadCallback)
        {
            adLoadCallback(new InterstitialAd($"gma-response-{Guid.NewGuid():N}"), null);
        }

        public void Show()
        {
        }

        public ResponseInfo GetResponseInfo() => _responseInfo;

        // ---- Stub-only simulation methods (not part of the real SDK) ----

        public void SimulateFullScreenContentOpened() => OnAdFullScreenContentOpened?.Invoke();
        public void SimulateImpressionRecorded() => OnAdImpressionRecorded?.Invoke();
        public void SimulateClicked() => OnAdClicked?.Invoke();
        public void SimulatePaid(AdValue adValue) => OnAdPaid?.Invoke(adValue);
        public void SimulateFullScreenContentClosed() => OnAdFullScreenContentClosed?.Invoke();
        public void SimulateFullScreenContentFailed(AdError error) => OnAdFullScreenContentFailed?.Invoke(error);
    }
}
