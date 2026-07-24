namespace DesignPatterns.MVC.Sample
{
    /// <summary>
    /// The Controller: it translates UI intents ("player pressed +", "clicked
    /// cycle difficulty") into model operations. It holds the model and nothing
    /// else — no view reference — so input handling and display stay separate.
    /// </summary>
    public sealed class SettingsController : Controller<GameSettings>
    {
        private const int VolumeStep = 10;

        public SettingsController(GameSettings model) : base(model)
        {
        }

        public void IncreaseVolume() => Model.SetMasterVolume(Model.MasterVolume + VolumeStep);

        public void DecreaseVolume() => Model.SetMasterVolume(Model.MasterVolume - VolumeStep);

        public void CycleDifficulty()
        {
            var next = Model.Difficulty switch
            {
                Difficulty.Easy => Difficulty.Normal,
                Difficulty.Normal => Difficulty.Hard,
                _ => Difficulty.Easy
            };

            Model.SetDifficulty(next);
        }

        public void ToggleFullscreen() => Model.SetFullscreen(!Model.IsFullscreen);
    }
}
