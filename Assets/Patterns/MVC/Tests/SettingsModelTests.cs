using NUnit.Framework;
using DesignPatterns.MVC.Sample;

namespace DesignPatterns.MVC.Tests
{
    public class SettingsModelTests
    {
        [Test]
        public void SetMasterVolume_ClampsToRange()
        {
            var model = new GameSettings();

            model.SetMasterVolume(999);
            Assert.AreEqual(100, model.MasterVolume);

            model.SetMasterVolume(-50);
            Assert.AreEqual(0, model.MasterVolume);
        }

        [Test]
        public void ChangingAValue_RaisesChanged()
        {
            var model = new GameSettings();
            var notifications = 0;
            model.Changed += () => notifications++;

            model.SetMasterVolume(80);

            Assert.AreEqual(1, notifications);
        }

        [Test]
        public void SettingTheSameValue_DoesNotRaiseChanged()
        {
            var model = new GameSettings();
            var notifications = 0;
            model.Changed += () => notifications++;

            model.SetMasterVolume(model.MasterVolume); // unchanged
            model.SetDifficulty(model.Difficulty);
            model.SetFullscreen(model.IsFullscreen);

            Assert.AreEqual(0, notifications);
        }

        [Test]
        public void ClampedNoOp_DoesNotRaiseChanged()
        {
            var model = new GameSettings();
            model.SetMasterVolume(100); // now at max
            var notifications = 0;
            model.Changed += () => notifications++;

            model.SetMasterVolume(200); // clamps to 100 — same as current, so no notification

            Assert.AreEqual(0, notifications);
        }
    }
}
