using UnityEngine;

namespace DesignPatterns.Component.Sample
{
    /// <summary>
    /// The integration domain: each tick it advances the shared
    /// <see cref="TransformComponent"/> by its velocity. It knows nothing about
    /// WHY the velocity is what it is — that's another component's job.
    /// </summary>
    public sealed class MoverComponent : EntityComponent, IUpdatable
    {
        private TransformComponent _transform;

        public void Update(float deltaTime)
        {
            // Resolve the sibling lazily, so add-order doesn't matter.
            _transform ??= Owner.RequireComponent<TransformComponent>();
            _transform.Position += _transform.Velocity * deltaTime;
        }
    }
}
