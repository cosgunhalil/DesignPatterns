namespace DesignPatterns.EventQueue.Sample
{
    /// <summary>
    /// A request to play a sound — the payload buffered on the queue. It's just
    /// data: the code that raises it (combat, pickups, UI) neither plays audio nor
    /// knows when the request will be handled.
    /// </summary>
    public readonly struct SoundEvent
    {
        public string SoundId { get; }
        public float Volume { get; }

        public SoundEvent(string soundId, float volume = 1f)
        {
            SoundId = soundId;
            Volume = volume;
        }

        public override string ToString() => $"{SoundId}@{Volume:0.0}";
    }
}
