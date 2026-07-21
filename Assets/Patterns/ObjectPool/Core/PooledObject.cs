using System;

namespace DesignPatterns.ObjectPool
{
    /// <summary>
    /// A borrow scope: dispose it (e.g. leave a <c>using</c> block) and the
    /// instance is released back to its pool automatically. This makes
    /// "forgot to Release" mistakes structurally hard, the same way a disposable
    /// subscription makes "forgot to unsubscribe" hard.
    /// </summary>
    /// <typeparam name="T">The pooled reference type.</typeparam>
    public readonly struct PooledObject<T> : IDisposable where T : class
    {
        private readonly T _value;
        private readonly IObjectPool<T> _pool;

        internal PooledObject(T value, IObjectPool<T> pool)
        {
            _value = value;
            _pool = pool;
        }

        public void Dispose() => _pool.Release(_value);
    }
}
