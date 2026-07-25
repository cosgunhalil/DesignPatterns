using System.Collections.Generic;
using NUnit.Framework;
using DesignPatterns.EventQueue.Sample;

namespace DesignPatterns.EventQueue.Tests
{
    public class AudioSystemTests
    {
        private AudioSystem _audio;
        private List<string> _played;

        [SetUp]
        public void SetUp()
        {
            _audio = new AudioSystem();
            _played = new List<string>();
            _audio.SoundPlayed += sound => _played.Add(sound.SoundId);
        }

        [Test]
        public void DuplicateSoundsInAFrame_PlayOnce()
        {
            _audio.BeginFrame();

            _audio.Handle(new SoundEvent("coin"));
            _audio.Handle(new SoundEvent("coin"));
            _audio.Handle(new SoundEvent("hit"));
            _audio.Handle(new SoundEvent("coin"));

            CollectionAssert.AreEqual(new[] { "coin", "hit" }, _played);
        }

        [Test]
        public void ANewFrame_AllowsTheSameSoundAgain()
        {
            _audio.BeginFrame();
            _audio.Handle(new SoundEvent("coin"));

            _audio.BeginFrame(); // next frame
            _audio.Handle(new SoundEvent("coin"));

            CollectionAssert.AreEqual(new[] { "coin", "coin" }, _played);
        }

        [Test]
        public void EndToEnd_QueueBatchesAFrameAndAudioMergesDuplicates()
        {
            var queue = new EventQueue<SoundEvent>();
            queue.Subscribe(_audio.Handle);

            // A frame's worth of gameplay raises requests, with duplicates.
            queue.Enqueue(new SoundEvent("footstep"));
            queue.Enqueue(new SoundEvent("coin"));
            queue.Enqueue(new SoundEvent("coin"));
            queue.Enqueue(new SoundEvent("footstep"));

            _audio.BeginFrame();
            queue.Process();

            CollectionAssert.AreEqual(new[] { "footstep", "coin" }, _played);
            Assert.AreEqual(0, queue.PendingCount);
        }
    }
}
