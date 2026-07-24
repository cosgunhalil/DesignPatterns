using UnityEngine;

namespace DesignPatterns.MVC.Sample
{
    /// <summary>
    /// The View: it renders the model's current state (here, to the Console; in a
    /// real screen it would drive UI Text/Sliders). It only reads the model and
    /// never changes it — that keeps it a pure function of model state, easy to
    /// swap for a different presentation without touching model or controller.
    /// </summary>
    public sealed class SettingsView : IView<GameSettings>
    {
        public void Render(GameSettings model)
        {
            Debug.Log($"<b>[Settings]</b> Volume {model.MasterVolume} | " +
                      $"Difficulty {model.Difficulty} | " +
                      $"Fullscreen {(model.IsFullscreen ? "On" : "Off")}");
        }
    }
}
