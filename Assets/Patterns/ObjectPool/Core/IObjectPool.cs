namespace DesignPatterns.ObjectPool
{
    /// <summary>
    /// Hands out reusable instances instead of allocating fresh ones. Callers
    /// <see cref="Get"/> an object, use it, then <see cref="Release"/> it back for
    /// the next caller — trading allocation/GC churn for a small amount of
    /// bookkeeping. The pool owns creation and reset; callers own only the borrow.
    /// </summary>
    /// <typeparam name="T">The pooled reference type.</typeparam>
    public interface IObjectPool<T> where T : class
    {
        /// <summary>Instances currently checked out (borrowed).</summary>
        int CountActive { get; }

        /// <summary>Instances sitting idle in the pool, ready to reuse.</summary>
        int CountInactive { get; }

        /// <summary>Every instance the pool has created and still tracks.</summary>
        int CountAll { get; }

        /// <summary>Borrow an instance — reused if one is idle, otherwise created.</summary>
        T Get();

        /// <summary>Borrow an instance wrapped in a scope that releases it when disposed.</summary>
        PooledObject<T> Get(out T value);

        /// <summary>Return a borrowed instance to the pool for reuse.</summary>
        void Release(T element);

        /// <summary>Drop and destroy every idle instance.</summary>
        void Clear();
    }
}
