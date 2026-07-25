using UnityEngine;

namespace DesignPatterns.State.Sample
{
    /// <summary>
    /// Entry point. Press Play — it runs a game-flow state machine. Press
    /// <b>Enter</b> (Confirm) and <b>Esc</b> (Cancel) and watch the SAME two keys
    /// do different things depending on the state: Confirm starts, then pauses,
    /// then resumes, then restarts; Cancel ends a run or backs out to the menu.
    /// The demo only presents transitions — all flow logic lives in the states.
    /// </summary>
    public sealed class StatePatternDemo : MonoBehaviour
    {
        private Game _game;
        private StateMachine<Game> _machine;

        private void Start()
        {
            _game = new Game();
            _machine = new StateMachine<Game>(_game, new MainMenuState());
            _machine.StateChanged += LogTransition;

            Debug.Log("<b>State demo</b> — Enter = Confirm, Esc = Cancel. One input, state-dependent behavior.");
            LogHint(_machine.CurrentState);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
            {
                _game.PendingCommand = GameCommand.Confirm;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                _game.PendingCommand = GameCommand.Cancel;
            }

            _machine.Update();
        }

        private void LogTransition(IState<Game> from, IState<Game> to)
        {
            Debug.Log($"<color=cyan>{from.GetType().Name} → {to.GetType().Name}</color>  " +
                      $"(score {_game.Score}, high {_game.HighScore})");
            LogHint(to);
        }

        private static void LogHint(IState<Game> state)
        {
            var hint = state switch
            {
                MainMenuState => "Main Menu — Confirm: start",
                PlayingState => "Playing — Confirm: pause · Cancel: end run",
                PausedState => "Paused — Confirm: resume · Cancel: quit to menu",
                GameOverState => "Game Over — Confirm: play again · Cancel: menu",
                _ => string.Empty
            };
            Debug.Log($"<color=grey>{hint}</color>");
        }

        private void OnDestroy()
        {
            if (_machine != null)
            {
                _machine.StateChanged -= LogTransition;
            }
        }
    }
}
