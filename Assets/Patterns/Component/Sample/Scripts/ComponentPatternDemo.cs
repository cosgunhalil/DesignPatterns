using UnityEngine;

namespace DesignPatterns.Component.Sample
{
    /// <summary>
    /// Entry point. Press Play — it assembles a "Guard" entity from four
    /// independent components and ticks it every frame; a capsule follows the
    /// entity's transform as it patrols. Press Space to damage the guard: when
    /// its health hits zero, the movement components are removed at runtime and
    /// it stops — showing behavior added and taken away by composition, not
    /// inheritance.
    ///
    /// This is deliberately the same shape as Unity's own GameObject: an entity
    /// that IS a bag of components with an Update loop.
    /// </summary>
    public sealed class ComponentPatternDemo : MonoBehaviour
    {
        private Entity _guard;
        private GameObject _view;

        private void Start()
        {
            CreateGround();

            _view = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _view.name = "Guard";

            _guard = new Entity("Guard");
            _guard.AddComponent(new TransformComponent { Position = new Vector3(-3f, 1f, 0f) });
            _guard.AddComponent(new PatrolComponent(-3f, 3f, 2f)); // decides velocity...
            _guard.AddComponent(new MoverComponent());             // ...ticked after, integrates it
            _guard.AddComponent(new HealthComponent(100));

            Debug.Log("Component demo — one Entity, four independent components. Space to damage the guard.");
        }

        private void Update()
        {
            _guard.Update(Time.deltaTime);
            _view.transform.position = _guard.RequireComponent<TransformComponent>().Position;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                var health = _guard.RequireComponent<HealthComponent>();
                health.TakeDamage(35);
                Debug.Log($"<color=orange>Guard hit:</color> {health.Current}/{health.Max}");

                if (!health.IsAlive)
                {
                    _guard.RemoveComponent<PatrolComponent>();
                    _guard.RemoveComponent<MoverComponent>();
                    Debug.Log("<color=red>Guard died</color> — movement components removed; it stops where it fell.");
                }
            }
        }

        private static void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(4f, 1f, 4f);
        }
    }
}
