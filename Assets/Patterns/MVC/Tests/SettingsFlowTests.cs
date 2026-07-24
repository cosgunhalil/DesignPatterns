using NUnit.Framework;
using DesignPatterns.MVC.Sample;

namespace DesignPatterns.MVC.Tests
{
    public class SettingsFlowTests
    {
        [Test]
        public void ControllerIntent_FlowsThroughModel_ToTheView()
        {
            var model = new GameSettings();
            var view = new RecordingView<GameSettings>();
            var controller = new SettingsController(model);
            using var binder = new ViewBinder<GameSettings>(model, view);

            controller.IncreaseVolume();

            Assert.AreEqual(60, model.MasterVolume);          // model updated
            Assert.AreEqual(60, view.Last.MasterVolume);      // view saw the update
            Assert.AreEqual(2, view.RenderCount);             // initial + one change
        }

        [Test]
        public void IncreaseVolume_ClampsAtMax()
        {
            var model = new GameSettings();
            var controller = new SettingsController(model);

            for (var i = 0; i < 20; i++)
            {
                controller.IncreaseVolume();
            }

            Assert.AreEqual(100, model.MasterVolume);
        }

        [Test]
        public void DecreaseVolume_ClampsAtZero()
        {
            var model = new GameSettings();
            var controller = new SettingsController(model);

            for (var i = 0; i < 20; i++)
            {
                controller.DecreaseVolume();
            }

            Assert.AreEqual(0, model.MasterVolume);
        }

        [Test]
        public void CycleDifficulty_WrapsThroughAllValues()
        {
            var model = new GameSettings(); // starts Normal
            var controller = new SettingsController(model);

            controller.CycleDifficulty();
            Assert.AreEqual(Difficulty.Hard, model.Difficulty);

            controller.CycleDifficulty();
            Assert.AreEqual(Difficulty.Easy, model.Difficulty);

            controller.CycleDifficulty();
            Assert.AreEqual(Difficulty.Normal, model.Difficulty);
        }

        [Test]
        public void ToggleFullscreen_Flips()
        {
            var model = new GameSettings(); // starts On
            var controller = new SettingsController(model);

            controller.ToggleFullscreen();
            Assert.IsFalse(model.IsFullscreen);

            controller.ToggleFullscreen();
            Assert.IsTrue(model.IsFullscreen);
        }
    }
}
