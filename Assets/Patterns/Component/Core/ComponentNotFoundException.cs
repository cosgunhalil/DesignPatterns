using System;

namespace DesignPatterns.Component
{
    /// <summary>
    /// Thrown by <see cref="Entity.RequireComponent{T}"/> when a required
    /// component is missing. Use <see cref="Entity.GetComponent{T}"/> or
    /// <see cref="Entity.TryGetComponent{T}"/> when absence is an expected case.
    /// </summary>
    public sealed class ComponentNotFoundException : Exception
    {
        public Type ComponentType { get; }

        public ComponentNotFoundException(Type componentType)
            : base($"The entity has no component of type '{componentType.Name}'.")
        {
            ComponentType = componentType;
        }
    }
}
