namespace DesignPatterns.Observer.Sample
{
    /// <summary>
    /// Immutable notification payload broadcast whenever health changes. Carrying
    /// before/after values (not just "health changed") lets each observer decide
    /// what it cares about — the delta, the fraction, or simply that it hit zero —
    /// without querying back into the subject.
    /// </summary>
    public struct HealthChanged
    {
        public int Previous { get; init; }
        public int Current { get; init; }
        public int Max { get; init; }

        public int Delta => Current - Previous;
        public bool IsDead => Current <= 0;
        public float Fraction => Max <= 0 ? 0f : (float)Current / Max;
    }
}
