namespace DesignPatterns.State.Sample
{
    // The four game-flow states. Notice how the SAME input (Confirm) does a
    // completely different thing in each — start, pause, resume, restart — with
    // no shared switch statement. That is the whole point of the State pattern.
    //
    // These states are pure logic (no logging/UI); the demo observes
    // StateMachine.StateChanged to present transitions.

    /// <summary>Idle at the title screen. Confirm starts a fresh game.</summary>
    public sealed class MainMenuState : State<Game>
    {
        public override void Update(StateMachine<Game> machine)
        {
            if (machine.Context.TakeCommand() == GameCommand.Confirm)
            {
                machine.Context.ResetScore();
                machine.ChangeState(new PlayingState());
            }
        }
    }

    /// <summary>
    /// Active gameplay. Score climbs each tick; Confirm pauses, Cancel ends the
    /// run (banking the high score).
    /// </summary>
    public sealed class PlayingState : State<Game>
    {
        public override void Update(StateMachine<Game> machine)
        {
            switch (machine.Context.TakeCommand())
            {
                case GameCommand.Confirm:
                    machine.ChangeState(new PausedState());
                    break;
                case GameCommand.Cancel:
                    machine.Context.CommitHighScore();
                    machine.ChangeState(new GameOverState());
                    break;
                default:
                    machine.Context.AddScore(1); // a tick of "play"
                    break;
            }
        }
    }

    /// <summary>Paused mid-run. Confirm resumes (score preserved); Cancel abandons to the menu.</summary>
    public sealed class PausedState : State<Game>
    {
        public override void Update(StateMachine<Game> machine)
        {
            switch (machine.Context.TakeCommand())
            {
                case GameCommand.Confirm:
                    machine.ChangeState(new PlayingState()); // resume — deliberately no ResetScore
                    break;
                case GameCommand.Cancel:
                    machine.ChangeState(new MainMenuState());
                    break;
            }
        }
    }

    /// <summary>Run finished. Confirm plays again from zero; Cancel returns to the menu.</summary>
    public sealed class GameOverState : State<Game>
    {
        public override void Update(StateMachine<Game> machine)
        {
            switch (machine.Context.TakeCommand())
            {
                case GameCommand.Confirm:
                    machine.Context.ResetScore();
                    machine.ChangeState(new PlayingState());
                    break;
                case GameCommand.Cancel:
                    machine.ChangeState(new MainMenuState());
                    break;
            }
        }
    }
}
