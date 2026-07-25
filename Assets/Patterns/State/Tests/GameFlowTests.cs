using NUnit.Framework;
using DesignPatterns.State.Sample;

namespace DesignPatterns.State.Tests
{
    public class GameFlowTests
    {
        private Game _game;
        private StateMachine<Game> _machine;

        [SetUp]
        public void SetUp()
        {
            _game = new Game();
            _machine = new StateMachine<Game>(_game, new MainMenuState());
        }

        private void Send(GameCommand command)
        {
            _game.PendingCommand = command;
            _machine.Update();
        }

        [Test]
        public void StartsInMainMenu()
        {
            Assert.IsTrue(_machine.IsInState<MainMenuState>());
        }

        [Test]
        public void Confirm_FromMenu_StartsAFreshGame()
        {
            Send(GameCommand.Confirm);

            Assert.IsTrue(_machine.IsInState<PlayingState>());
            Assert.AreEqual(0, _game.Score);
        }

        [Test]
        public void Playing_WithoutInput_AccumulatesScore()
        {
            Send(GameCommand.Confirm); // -> Playing
            _machine.Update();         // a tick of play
            _machine.Update();

            Assert.AreEqual(2, _game.Score);
        }

        [Test]
        public void SameInput_MeansDifferentThings_PerState()
        {
            // Confirm: start...
            Send(GameCommand.Confirm);
            Assert.IsTrue(_machine.IsInState<PlayingState>());

            // ...then pause...
            Send(GameCommand.Confirm);
            Assert.IsTrue(_machine.IsInState<PausedState>());

            // ...then resume.
            Send(GameCommand.Confirm);
            Assert.IsTrue(_machine.IsInState<PlayingState>());
        }

        [Test]
        public void Resume_FromPause_PreservesScore()
        {
            Send(GameCommand.Confirm); // Playing
            _machine.Update();         // score 1
            _machine.Update();         // score 2
            Send(GameCommand.Confirm); // Paused
            Send(GameCommand.Confirm); // resume -> Playing

            Assert.IsTrue(_machine.IsInState<PlayingState>());
            Assert.AreEqual(2, _game.Score, "resuming must not reset the score");
        }

        [Test]
        public void Cancel_FromPlaying_EndsRunAndBanksHighScore()
        {
            Send(GameCommand.Confirm); // Playing
            _machine.Update();         // score 1
            _machine.Update();         // score 2
            Send(GameCommand.Cancel);  // -> GameOver

            Assert.IsTrue(_machine.IsInState<GameOverState>());
            Assert.AreEqual(2, _game.HighScore);
        }

        [Test]
        public void Restart_FromGameOver_ResetsScore()
        {
            Send(GameCommand.Confirm); // Playing
            _machine.Update();
            Send(GameCommand.Cancel);  // GameOver (score 1 banked)
            Send(GameCommand.Confirm); // restart -> Playing

            Assert.IsTrue(_machine.IsInState<PlayingState>());
            Assert.AreEqual(0, _game.Score);
        }

        [Test]
        public void HighScore_KeepsTheBestAcrossRuns()
        {
            // Run 1: score 3, end.
            Send(GameCommand.Confirm);
            _machine.Update();
            _machine.Update();
            _machine.Update();
            Send(GameCommand.Cancel); // GameOver, high = 3

            // Run 2: score 1, end.
            Send(GameCommand.Confirm); // restart
            _machine.Update();
            Send(GameCommand.Cancel);  // GameOver, high stays 3

            Assert.AreEqual(3, _game.HighScore);
        }

        [Test]
        public void Cancel_FromPause_ReturnsToMenu()
        {
            Send(GameCommand.Confirm); // Playing
            Send(GameCommand.Confirm); // Paused
            Send(GameCommand.Cancel);  // -> Main Menu

            Assert.IsTrue(_machine.IsInState<MainMenuState>());
        }
    }
}
