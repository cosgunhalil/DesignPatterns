namespace DesignPatterns.State.Sample
{
    /// <summary>The minimal player input vocabulary the flow states react to.</summary>
    public enum GameCommand
    {
        None,
        Confirm,
        Cancel
    }

    /// <summary>
    /// The shared context the states operate on: the game's score data plus the
    /// latest pending command. It's plain C# (no engine dependency), so the whole
    /// state machine is testable without a running scene.
    /// </summary>
    public sealed class Game
    {
        public int Score { get; private set; }
        public int HighScore { get; private set; }

        /// <summary>Set by the input layer; consumed by the active state via <see cref="TakeCommand"/>.</summary>
        public GameCommand PendingCommand { get; set; }

        public void AddScore(int amount) => Score += amount;

        public void ResetScore() => Score = 0;

        public void CommitHighScore()
        {
            if (Score > HighScore)
            {
                HighScore = Score;
            }
        }

        /// <summary>Return the pending command and clear it, so it's handled exactly once.</summary>
        public GameCommand TakeCommand()
        {
            var command = PendingCommand;
            PendingCommand = GameCommand.None;
            return command;
        }
    }
}
