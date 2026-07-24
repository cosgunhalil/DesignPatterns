using UnityEngine;

namespace DesignPatterns.MVC.Sample
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    /// <summary>
    /// The Model: the settings state plus the rules that keep it valid (volume
    /// stays in 0–100). Each mutator changes state and notifies only when the
    /// value truly changed, so views never re-render for a no-op. It knows
    /// nothing about the view or controller.
    /// </summary>
    public sealed class GameSettings : ObservableModel
    {
        public int MasterVolume { get; private set; } = 50;
        public Difficulty Difficulty { get; private set; } = Difficulty.Normal;
        public bool IsFullscreen { get; private set; } = true;

        public void SetMasterVolume(int value)
        {
            var clamped = Mathf.Clamp(value, 0, 100);
            if (clamped == MasterVolume)
            {
                return;
            }

            MasterVolume = clamped;
            NotifyChanged();
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            if (difficulty == Difficulty)
            {
                return;
            }

            Difficulty = difficulty;
            NotifyChanged();
        }

        public void SetFullscreen(bool value)
        {
            if (value == IsFullscreen)
            {
                return;
            }

            IsFullscreen = value;
            NotifyChanged();
        }
    }
}
