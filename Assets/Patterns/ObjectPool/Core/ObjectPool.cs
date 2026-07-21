using System;
using System.Collections.Generic;

namespace DesignPatterns.ObjectPool
{
    /// <summary>
    /// A generic, engine-free object pool. Creation, reset-on-borrow,
    /// reset-on-return, and destruction are supplied as delegates, so the same
    /// pool works for plain C# objects, Unity components, network buffers —
    /// anything expensive to allocate repeatedly.
    ///
    /// Unity 6 ships its own <c>UnityEngine.Pool.ObjectPool&lt;T&gt;</c>; this
    /// from-scratch version exists to show the mechanics. Prefer the built-in one
    /// in production, and reach for this understanding when you need something it
    /// doesn't cover.
    /// </summary>
    /// <typeparam name="T">The pooled reference type.</typeparam>
    public sealed class ObjectPool<T> : IObjectPool<T> where T : class
    {
        private readonly Stack<T> _idle = new();
        private readonly Func<T> _createFunc;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;
        private readonly Action<T> _onDestroy;
        private readonly int _maxSize;
        private readonly bool _collectionCheck;

        /// <param name="createFunc">Makes a brand-new instance when none are idle.</param>
        /// <param name="onGet">Reset applied as an instance is borrowed (e.g. activate).</param>
        /// <param name="onRelease">Reset applied as an instance is returned (e.g. deactivate).</param>
        /// <param name="onDestroy">Disposal for instances the pool discards (over capacity, or on Clear).</param>
        /// <param name="maxSize">Idle instances are capped here; extras returned beyond it are destroyed.</param>
        /// <param name="collectionCheck">When true, releasing an instance already idle throws instead of corrupting the pool.</param>
        public ObjectPool(
            Func<T> createFunc,
            Action<T> onGet = null,
            Action<T> onRelease = null,
            Action<T> onDestroy = null,
            int maxSize = 1000,
            bool collectionCheck = true)
        {
            if (maxSize <= 0)
            {
                throw new ArgumentException("Max size must be greater than zero.", nameof(maxSize));
            }

            _createFunc = createFunc ?? throw new ArgumentNullException(nameof(createFunc));
            _onGet = onGet;
            _onRelease = onRelease;
            _onDestroy = onDestroy;
            _maxSize = maxSize;
            _collectionCheck = collectionCheck;
        }

        public int CountAll { get; private set; }
        public int CountInactive => _idle.Count;
        public int CountActive => CountAll - CountInactive;

        public T Get()
        {
            T element;
            if (_idle.Count == 0)
            {
                element = _createFunc();
                CountAll++;
            }
            else
            {
                element = _idle.Pop();
            }

            _onGet?.Invoke(element);
            return element;
        }

        public PooledObject<T> Get(out T value)
        {
            value = Get();
            return new PooledObject<T>(value, this);
        }

        public void Release(T element)
        {
            if (element == null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            if (_collectionCheck && _idle.Contains(element))
            {
                throw new InvalidOperationException("This instance is already released to the pool (double release).");
            }

            _onRelease?.Invoke(element);

            if (_idle.Count < _maxSize)
            {
                _idle.Push(element);
            }
            else
            {
                // Pool is full: this instance is surplus, so let it go.
                _onDestroy?.Invoke(element);
                CountAll--;
            }
        }

        /// <summary>Pre-create <paramref name="count"/> idle instances so early Gets don't allocate.</summary>
        public void Prewarm(int count)
        {
            for (var i = 0; i < count && _idle.Count < _maxSize; i++)
            {
                var element = _createFunc();
                CountAll++;
                _onRelease?.Invoke(element); // put it in the idle/reset state
                _idle.Push(element);
            }
        }

        public void Clear()
        {
            if (_onDestroy != null)
            {
                foreach (var element in _idle)
                {
                    _onDestroy(element);
                }
            }

            CountAll -= _idle.Count;
            _idle.Clear();
        }
    }
}
