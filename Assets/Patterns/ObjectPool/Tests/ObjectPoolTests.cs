using System;
using NUnit.Framework;

namespace DesignPatterns.ObjectPool.Tests
{
    public class ObjectPoolTests
    {
        private int _created;

        private ObjectPool<TrackedResource> NewPool(int maxSize = 1000, bool collectionCheck = true)
        {
            _created = 0;
            return new ObjectPool<TrackedResource>(
                createFunc: () =>
                {
                    _created++;
                    return new TrackedResource();
                },
                onGet: r => r.GetCount++,
                onRelease: r => r.ReleaseCount++,
                onDestroy: r => r.Destroyed = true,
                maxSize: maxSize,
                collectionCheck: collectionCheck);
        }

        [Test]
        public void Get_WhenEmpty_CreatesNewInstance()
        {
            var pool = NewPool();

            var item = pool.Get();

            Assert.IsNotNull(item);
            Assert.AreEqual(1, _created);
            Assert.AreEqual(1, pool.CountAll);
            Assert.AreEqual(1, pool.CountActive);
            Assert.AreEqual(0, pool.CountInactive);
        }

        [Test]
        public void Release_ReturnsInstanceToTheIdleSet()
        {
            var pool = NewPool();
            var item = pool.Get();

            pool.Release(item);

            Assert.AreEqual(1, pool.CountAll);
            Assert.AreEqual(0, pool.CountActive);
            Assert.AreEqual(1, pool.CountInactive);
        }

        [Test]
        public void Get_AfterRelease_ReusesTheSameInstance()
        {
            var pool = NewPool();
            var first = pool.Get();
            pool.Release(first);

            var second = pool.Get();

            Assert.AreSame(first, second);
            Assert.AreEqual(1, _created, "no new instance should be created when one is idle");
        }

        [Test]
        public void GetAndRelease_InvokeTheHooks()
        {
            var pool = NewPool();

            var item = pool.Get();
            pool.Release(item);

            Assert.AreEqual(1, item.GetCount);
            Assert.AreEqual(1, item.ReleaseCount);
        }

        [Test]
        public void Release_BeyondMaxSize_DestroysInsteadOfPooling()
        {
            var pool = NewPool(maxSize: 1);
            var a = pool.Get();
            var b = pool.Get();

            pool.Release(a); // fits (idle 0 -> 1)
            pool.Release(b); // idle already at max -> destroyed

            Assert.AreEqual(1, pool.CountInactive);
            Assert.IsTrue(b.Destroyed);
            Assert.IsFalse(a.Destroyed);
            Assert.AreEqual(1, pool.CountAll);
        }

        [Test]
        public void Prewarm_CreatesIdleInstances()
        {
            var pool = NewPool();

            pool.Prewarm(3);

            Assert.AreEqual(3, _created);
            Assert.AreEqual(3, pool.CountAll);
            Assert.AreEqual(3, pool.CountInactive);
            Assert.AreEqual(0, pool.CountActive);
        }

        [Test]
        public void Prewarm_RespectsMaxSize()
        {
            var pool = NewPool(maxSize: 2);

            pool.Prewarm(5);

            Assert.AreEqual(2, pool.CountInactive);
        }

        [Test]
        public void Clear_DestroysIdleInstancesAndResetsCounts()
        {
            var pool = NewPool();
            var a = pool.Get();
            var b = pool.Get();
            pool.Release(a);
            pool.Release(b);

            pool.Clear();

            Assert.AreEqual(0, pool.CountInactive);
            Assert.AreEqual(0, pool.CountAll);
            Assert.IsTrue(a.Destroyed);
            Assert.IsTrue(b.Destroyed);
        }

        [Test]
        public void Release_Twice_WithCollectionCheck_Throws()
        {
            var pool = NewPool();
            var item = pool.Get();
            pool.Release(item);

            Assert.Throws<InvalidOperationException>(() => pool.Release(item));
        }

        [Test]
        public void Release_Twice_WithoutCollectionCheck_DoesNotThrow()
        {
            var pool = NewPool(collectionCheck: false);
            var item = pool.Get();
            pool.Release(item);

            Assert.DoesNotThrow(() => pool.Release(item));
        }

        [Test]
        public void Release_Null_Throws()
        {
            var pool = NewPool();

            Assert.Throws<ArgumentNullException>(() => pool.Release(null));
        }

        [Test]
        public void Constructor_NullFactory_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new ObjectPool<TrackedResource>(null));
        }

        [Test]
        public void Constructor_NonPositiveMaxSize_Throws()
        {
            Assert.Throws<ArgumentException>(
                () => new ObjectPool<TrackedResource>(() => new TrackedResource(), maxSize: 0));
        }

        [Test]
        public void PooledObjectScope_ReleasesOnDispose()
        {
            var pool = NewPool();

            using (pool.Get(out var borrowed))
            {
                Assert.AreEqual(1, pool.CountActive);
                Assert.IsNotNull(borrowed);
            }

            Assert.AreEqual(0, pool.CountActive);
            Assert.AreEqual(1, pool.CountInactive);
        }

        [Test]
        public void CountsStayConsistentAcrossMixedOperations()
        {
            var pool = NewPool();
            var a = pool.Get();
            var b = pool.Get();
            var c = pool.Get();
            pool.Release(b);

            Assert.AreEqual(3, pool.CountAll);
            Assert.AreEqual(2, pool.CountActive); // a and c still out
            Assert.AreEqual(1, pool.CountInactive); // b idle

            GC.KeepAlive(a);
            GC.KeepAlive(c);
        }
    }
}
