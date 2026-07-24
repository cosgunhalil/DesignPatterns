using System;
using UnityEngine;

namespace DesignPatterns.MVC.Sample
{
    /// <summary>
    /// Entry point. Press Play — it wires a settings Model, View, and Controller,
    /// and a <see cref="ViewBinder{TModel}"/> keeps the view in sync. Use the keys
    /// to fire controller intents; each one mutates the model, the model raises
    /// Changed, and the view re-renders — the MVC round trip, once per keypress.
    ///
    /// Up/Down volume · D cycle difficulty · F toggle fullscreen
    /// </summary>
    public sealed class MvcPatternDemo : MonoBehaviour
    {
        private SettingsController _controller;
        private IDisposable _binding;

        private void Start()
        {
            var model = new GameSettings();
            var view = new SettingsView();

            _controller = new SettingsController(model);
            _binding = new ViewBinder<GameSettings>(model, view); // renders the initial state now

            Debug.Log("MVC demo — Up/Down volume · D cycle difficulty · F toggle fullscreen.");
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _controller.IncreaseVolume();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                _controller.DecreaseVolume();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                _controller.CycleDifficulty();
            }
            else if (Input.GetKeyDown(KeyCode.F))
            {
                _controller.ToggleFullscreen();
            }
        }

        private void OnDestroy() => _binding?.Dispose();
    }
}
