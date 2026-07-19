namespace DesignPatterns.Adapter
{
    /// <summary>
    /// The normalized advertisement lifecycle. Applicability by format:
    ///
    /// - Full-screen (Rewarded, Interstitial, AppOpen): Requested, Loaded,
    ///   LoadFailed, Displayed, ImpressionRecorded, Clicked, RevenuePaid,
    ///   Closed, DisplayFailed. Rewarded additionally uses Completed (the
    ///   provider's reward callback fired) and Cancelled (interaction
    ///   finalized after close without a reward callback).
    /// - Banner: Requested, Loaded, LoadFailed, ImpressionRecorded, Clicked,
    ///   RevenuePaid. Banners have no completion, cancellation, or close.
    /// - Native: same set as Banner.
    ///
    /// Semantic rules:
    /// - Completed IS the normalized "reward granted by provider" event; there
    ///   is deliberately no separate RewardGranted lifecycle event.
    /// - A rewarded ad may produce BOTH Completed and Closed — different facts.
    /// - Closed alone never implies Cancelled: providers may deliver the reward
    ///   callback after the close callback.
    /// - Requested is emitted by the game (via RecordRequest), not by provider
    ///   SDKs, because SDKs do not consistently expose a request callback.
    /// </summary>
    public enum AdLifecycleEvent
    {
        Requested,
        Loaded,
        LoadFailed,
        Displayed,
        ImpressionRecorded,
        Clicked,
        RevenuePaid,
        Completed,
        Cancelled,
        Closed,
        DisplayFailed
    }
}
