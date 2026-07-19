namespace DesignPatterns.Adapter
{
    /// <summary>
    /// Normalized advertisement formats. The domain can represent all five even
    /// though not every provider supports every format (see the capability
    /// matrix in the README).
    /// </summary>
    public enum AdFormat
    {
        Rewarded,
        Interstitial,
        Banner,
        AppOpen,
        Native
    }
}
