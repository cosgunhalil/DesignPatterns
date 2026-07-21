namespace DesignPatterns.ObjectPool.Tests
{
    /// <summary>Pooled test object that records how the pool's hooks touched it.</summary>
    internal sealed class TrackedResource
    {
        public int GetCount { get; set; }
        public int ReleaseCount { get; set; }
        public bool Destroyed { get; set; }
    }
}
