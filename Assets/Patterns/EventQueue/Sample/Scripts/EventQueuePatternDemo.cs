using UnityEngine;

namespace DesignPatterns.EventQueue.Sample
{
    /// <summary>
    /// Entry point. Press Play — number keys raise sound requests during a frame;
    /// the queue holds them and drains once per "audio frame" (every 0.5s here).
    /// Mash the same key and only one sound plays per frame: the requests were
    /// buffered and merged. Notice the gap in the log between "queued" and
    /// "played" — that gap is the whole point of the pattern.
    ///
    /// 1 = footstep · 2 = coin · 3 = hit
    /// </summary>
    public sealed class EventQueuePatternDemo : MonoBehaviour
    {
        private const float FrameInterval = 0.5f;

        private readonly EventQueue<SoundEvent> _queue = new();
        private readonly AudioSystem _audio = new();
        private float _timer;

        private void Start()
        {
            _queue.Subscribe(_audio.Handle);
            _audio.SoundPlayed += sound => Debug.Log($"<color=lime>♪ played {sound}</color>");

            Debug.Log("<b>Event Queue demo</b> — 1 footstep · 2 coin · 3 hit. Requests are drained every 0.5s; duplicates in a frame merge.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Queue(new SoundEvent("footstep"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Queue(new SoundEvent("coin"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Queue(new SoundEvent("hit"));
            }

            _timer += Time.deltaTime;
            if (_timer >= FrameInterval)
            {
                _timer -= FrameInterval;
                DrainAudioFrame();
            }
        }

        private void Queue(SoundEvent sound)
        {
            _queue.Enqueue(sound);
            Debug.Log($"<color=grey>queued {sound} (pending {_queue.PendingCount})</color>");
        }

        private void DrainAudioFrame()
        {
            if (_queue.PendingCount == 0)
            {
                return;
            }

            _audio.BeginFrame(); // reset the per-frame merge window
            _queue.Process();    // dispatch this frame's requests to the audio system
        }
    }
}
