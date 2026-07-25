using System;
using System.Collections.Generic;

namespace DesignPatterns.EventQueue.Sample
{
    /// <summary>
    /// The consumer end of the queue: it subscribes to processed <see cref="SoundEvent"/>s
    /// and decides what actually plays. Because the queue batches a whole frame's
    /// requests into one <see cref="EventQueue{TEvent}.Process"/>, this system can
    /// <b>merge duplicates</b> — five "coin" pickups in a frame become one coin
    /// sound. That aggregation is only possible because delivery was deferred and
    /// batched; a synchronous Observer would fire the sound five times.
    ///
    /// It raises <see cref="SoundPlayed"/> instead of playing directly, so the
    /// decision logic stays pure and testable (the demo wires this to the Console).
    /// </summary>
    public sealed class AudioSystem
    {
        private readonly HashSet<string> _playedThisFrame = new();

        /// <summary>Raised once per unique sound actually played in a frame.</summary>
        public event Action<SoundEvent> SoundPlayed;

        /// <summary>Reset the per-frame de-duplication window. Call once before processing a frame's queue.</summary>
        public void BeginFrame() => _playedThisFrame.Clear();

        /// <summary>Handle one processed request; skips a sound already played this frame.</summary>
        public void Handle(SoundEvent sound)
        {
            if (!_playedThisFrame.Add(sound.SoundId))
            {
                return; // duplicate within the frame — merged away
            }

            SoundPlayed?.Invoke(sound);
        }
    }
}
