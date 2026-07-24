using System;
using System.Collections.Generic;

namespace DesignPatterns.Component
{
    /// <summary>
    /// A container of components — the "entity" in Component/Entity-Component
    /// designs. It holds behavior by composition instead of inheritance: rather
    /// than a deep class hierarchy (<c>MovingDamageableEnemy</c>), you assemble
    /// an entity from independent components at runtime.
    ///
    /// This is exactly the shape of Unity's own <c>GameObject</c> +
    /// <c>MonoBehaviour</c> (AddComponent/GetComponent/the Update loop). Building
    /// it from scratch here shows the mechanics Unity hides.
    ///
    /// Queries return the first component assignable to the requested type, so
    /// you can ask by concrete type or by an interface a component implements.
    /// Components are ticked in the order they were added.
    /// </summary>
    public sealed class Entity
    {
        private readonly List<IComponent> _components = new();

        public string Name { get; }
        public int ComponentCount => _components.Count;

        public Entity(string name = "Entity")
        {
            Name = name;
        }

        /// <summary>Add a component and attach it to this entity.</summary>
        public T AddComponent<T>(T component) where T : class, IComponent
        {
            if (component == null)
            {
                throw new ArgumentNullException(nameof(component));
            }

            _components.Add(component);
            component.Attach(this);
            return component;
        }

        /// <summary>First component assignable to <typeparamref name="T"/>, or null.</summary>
        public T GetComponent<T>() where T : class
        {
            foreach (var component in _components)
            {
                if (component is T typed)
                {
                    return typed;
                }
            }

            return null;
        }

        public bool TryGetComponent<T>(out T component) where T : class
        {
            component = GetComponent<T>();
            return component != null;
        }

        public bool HasComponent<T>() where T : class => GetComponent<T>() != null;

        /// <summary>Like <see cref="GetComponent{T}"/> but throws when absent — for hard dependencies.</summary>
        public T RequireComponent<T>() where T : class =>
            GetComponent<T>() ?? throw new ComponentNotFoundException(typeof(T));

        /// <summary>Remove and detach the first component assignable to <typeparamref name="T"/>.</summary>
        public bool RemoveComponent<T>() where T : class
        {
            for (var i = 0; i < _components.Count; i++)
            {
                if (_components[i] is T)
                {
                    var removed = _components[i];
                    _components.RemoveAt(i);
                    removed.Detach();
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Tick every component that is <see cref="IUpdatable"/>, in add order.
        /// Iterates a snapshot so a component may add or remove components from
        /// inside its own Update without corrupting the loop.
        /// </summary>
        public void Update(float deltaTime)
        {
            var snapshot = _components.ToArray();
            foreach (var component in snapshot)
            {
                if (component is IUpdatable updatable)
                {
                    updatable.Update(deltaTime);
                }
            }
        }
    }
}
