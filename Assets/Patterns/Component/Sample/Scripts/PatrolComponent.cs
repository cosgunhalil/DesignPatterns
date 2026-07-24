using UnityEngine;

namespace DesignPatterns.Component.Sample
{
    /// <summary>
    /// The AI domain: it steers the entity back and forth between two X bounds by
    /// setting the shared <see cref="TransformComponent"/>'s velocity. The
    /// <see cref="MoverComponent"/> then does the actual moving. Two components,
    /// two domains (decide vs. integrate), collaborating only through the transform.
    /// </summary>
    public sealed class PatrolComponent : EntityComponent, IUpdatable
    {
        private readonly float _minX;
        private readonly float _maxX;
        private readonly float _speed;
        private TransformComponent _transform;

        public PatrolComponent(float minX, float maxX, float speed)
        {
            _minX = minX;
            _maxX = maxX;
            _speed = speed;
        }

        public void Update(float deltaTime)
        {
            _transform ??= Owner.RequireComponent<TransformComponent>();

            var velocity = _transform.Velocity;

            // Bounds win over the initial default, so an entity that starts at or
            // beyond a bound still turns the correct way on its first tick.
            if (_transform.Position.x >= _maxX)
            {
                velocity.x = -_speed;
            }
            else if (_transform.Position.x <= _minX)
            {
                velocity.x = _speed;
            }
            else if (velocity.x == 0f)
            {
                velocity.x = _speed; // default start direction while inside the bounds
            }

            _transform.Velocity = velocity;
        }
    }
}
