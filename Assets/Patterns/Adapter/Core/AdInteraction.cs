namespace DesignPatterns.Adapter
{
    /// <summary>
    /// Minimal per-interaction state. One interaction = one attempt to show one
    /// advertisement, correlated by a locally generated id because providers do
    /// not expose a reliable impression identifier across all callbacks.
    /// </summary>
    public sealed class AdInteraction
    {
        public string InteractionId { get; }
        public string AdUnitId { get; }
        public AdFormat Format { get; }
        public string PlacementId { get; internal set; }

        public bool WasRequested { get; internal set; }
        public bool WasDisplayed { get; internal set; }
        public bool RewardCallbackReceived { get; internal set; }
        public bool WasClosed { get; internal set; }
        public bool IsFinalized { get; internal set; }

        internal AdInteraction(string interactionId, string adUnitId, AdFormat format, string placementId)
        {
            InteractionId = interactionId;
            AdUnitId = adUnitId;
            Format = format;
            PlacementId = placementId;
        }
    }
}
