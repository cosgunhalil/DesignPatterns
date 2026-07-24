using UnityEngine;

namespace DesignPatterns.Component.Sample
{
    /// <summary>
    /// The spatial-state domain: where the entity is and how fast it's moving.
    /// It's passive (no Update) — other components read and write it, which is
    /// how they collaborate without referencing each other directly.
    /// </summary>
    public sealed class TransformComponent : EntityComponent
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
    }
}
