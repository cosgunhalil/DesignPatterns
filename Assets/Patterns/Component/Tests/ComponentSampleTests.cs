using NUnit.Framework;
using UnityEngine;
using DesignPatterns.Component.Sample;

namespace DesignPatterns.Component.Tests
{
    public class ComponentSampleTests
    {
        [Test]
        public void Mover_AdvancesPositionByVelocity()
        {
            var entity = new Entity("Mover");
            var transform = entity.AddComponent(new TransformComponent { Velocity = new Vector3(2f, 0f, 0f) });
            entity.AddComponent(new MoverComponent());

            entity.Update(0.5f);

            Assert.AreEqual(1f, transform.Position.x, 0.0001f);
        }

        [Test]
        public void Patrol_SetsInitialVelocityTowardMax()
        {
            var entity = new Entity("Patrol");
            var transform = entity.AddComponent(new TransformComponent { Position = new Vector3(0f, 0f, 0f) });
            entity.AddComponent(new PatrolComponent(-3f, 3f, 2f));

            entity.Update(0.016f);

            Assert.Greater(transform.Velocity.x, 0f);
        }

        [Test]
        public void Patrol_ReversesAtUpperBound()
        {
            var entity = new Entity("Patrol");
            var transform = entity.AddComponent(new TransformComponent { Position = new Vector3(5f, 0f, 0f) });
            entity.AddComponent(new PatrolComponent(-3f, 3f, 2f));

            entity.Update(0.016f);

            Assert.Less(transform.Velocity.x, 0f, "past the max bound it should steer back left");
        }

        [Test]
        public void PatrolAndMover_Collaborate_ToMoveThenTurnAround()
        {
            var entity = new Entity("Guard");
            var transform = entity.AddComponent(new TransformComponent { Position = new Vector3(2.9f, 0f, 0f) });
            entity.AddComponent(new PatrolComponent(-3f, 3f, 2f)); // decides
            entity.AddComponent(new MoverComponent());             // integrates

            // Step forward until it crosses the max bound, then one more tick to reverse.
            for (var i = 0; i < 10; i++)
            {
                entity.Update(0.1f);
            }

            Assert.Less(transform.Velocity.x, 0f);
        }

        [Test]
        public void RemovingMovement_StopsPositionChanges()
        {
            var entity = new Entity("Guard");
            var transform = entity.AddComponent(new TransformComponent { Velocity = new Vector3(2f, 0f, 0f) });
            entity.AddComponent(new PatrolComponent(-3f, 3f, 2f));
            entity.AddComponent(new MoverComponent());

            entity.RemoveComponent<PatrolComponent>();
            entity.RemoveComponent<MoverComponent>();
            var frozen = transform.Position;
            entity.Update(1f);

            Assert.AreEqual(frozen, transform.Position);
        }

        [Test]
        public void Health_IsAnIndependentDomain()
        {
            var entity = new Entity("Guard");
            var health = entity.AddComponent(new HealthComponent(100));

            health.TakeDamage(30);
            Assert.AreEqual(70, health.Current);
            Assert.IsTrue(health.IsAlive);

            health.TakeDamage(999);
            Assert.AreEqual(0, health.Current);
            Assert.IsFalse(health.IsAlive);

            health.Heal(50);
            Assert.AreEqual(50, health.Current);
        }
    }
}
