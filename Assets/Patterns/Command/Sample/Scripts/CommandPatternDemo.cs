using UnityEngine;

namespace DesignPatterns.Command.Sample
{
    /// <summary>
    /// Entry point: builds the scene objects, binds keys to commands and routes
    /// everything through a single <see cref="CommandInvoker"/>.
    ///
    /// WASD move · Space jump · C dash combo · P reset · Z undo · Y redo
    /// </summary>
    public class CommandPatternDemo : MonoBehaviour
    {
        private static readonly Vector3 SpawnPosition = new(0f, 0.5f, 0f);

        [SerializeField] private float moveStep = 1f;
        [SerializeField] private float jumpForce = 5f;

        private readonly CommandInvoker _invoker = new();
        private readonly InputHandler _input = new();
        private Player _player;

        private void Start()
        {
            CreateGround();
            _player = CreatePlayer();
            BindControls();

            Debug.Log("WASD move · Space jump · C dash combo · P reset · Z undo · Y redo");
        }

        private void Update()
        {
            // Undo/redo talk to the invoker's history rather than doing work
            // themselves, so they are requests to the invoker — not commands.
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (!_invoker.Undo())
                {
                    Debug.Log("Nothing to undo.");
                }

                return;
            }

            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (!_invoker.Redo())
                {
                    Debug.Log("Nothing to redo.");
                }

                return;
            }

            var command = _input.HandleInput();
            if (command != null)
            {
                _invoker.Execute(command);
            }
        }

        private void BindControls()
        {
            _input.Bind(KeyCode.W, () => new MoveCommand(_player, Vector3.forward * moveStep));
            _input.Bind(KeyCode.S, () => new MoveCommand(_player, Vector3.back * moveStep));
            _input.Bind(KeyCode.A, () => new MoveCommand(_player, Vector3.left * moveStep));
            _input.Bind(KeyCode.D, () => new MoveCommand(_player, Vector3.right * moveStep));
            _input.Bind(KeyCode.Space, () => new JumpCommand(_player, jumpForce));

            // A macro is just another command: this dash is two moves executed
            // together and undone as a single history entry.
            _input.Bind(KeyCode.C, () => new CompositeCommand(
                new MoveCommand(_player, Vector3.forward * moveStep),
                new MoveCommand(_player, Vector3.forward * moveStep)));

            // One-off actions fit in a RelayCommand — no class needed.
            _input.Bind(KeyCode.P, () => new RelayCommand<Player>(
                player =>
                {
                    player.Position = SpawnPosition;
                    Debug.Log("<color=magenta>Reset to spawn.</color>");
                },
                _player));
        }

        private Player CreatePlayer()
        {
            var playerObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            playerObject.name = "Player";
            playerObject.transform.position = SpawnPosition;

            var body = playerObject.AddComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.FreezeRotation;

            return playerObject.AddComponent<Player>();
        }

        private static void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4f, 1f, 4f);
        }
    }
}
