using NUnit.Framework;

namespace DesignPatterns.Component.Tests
{
    public class EntityTests
    {
        private Entity _entity;

        [SetUp]
        public void SetUp()
        {
            _entity = new Entity("Test");
        }

        [Test]
        public void AddComponent_StoresAndAttachesIt()
        {
            var tag = _entity.AddComponent(new TagComponent());

            Assert.AreSame(tag, _entity.GetComponent<TagComponent>());
            Assert.AreSame(_entity, tag.Owner);
            Assert.AreEqual(1, _entity.ComponentCount);
        }

        [Test]
        public void AddComponent_Null_Throws()
        {
            Assert.Throws<System.ArgumentNullException>(() => _entity.AddComponent<TagComponent>(null));
        }

        [Test]
        public void GetComponent_Missing_ReturnsNull()
        {
            Assert.IsNull(_entity.GetComponent<TagComponent>());
        }

        [Test]
        public void TryGetComponent_ReflectsPresence()
        {
            Assert.IsFalse(_entity.TryGetComponent<TagComponent>(out _));

            _entity.AddComponent(new TagComponent());

            Assert.IsTrue(_entity.TryGetComponent<TagComponent>(out var tag));
            Assert.IsNotNull(tag);
        }

        [Test]
        public void HasComponent_ReflectsPresence()
        {
            Assert.IsFalse(_entity.HasComponent<TagComponent>());
            _entity.AddComponent(new TagComponent());
            Assert.IsTrue(_entity.HasComponent<TagComponent>());
        }

        [Test]
        public void RequireComponent_Missing_Throws()
        {
            var ex = Assert.Throws<ComponentNotFoundException>(() => _entity.RequireComponent<TagComponent>());
            Assert.AreEqual(typeof(TagComponent), ex.ComponentType);
        }

        [Test]
        public void GetComponent_MatchesByCapabilityInterface()
        {
            _entity.AddComponent(new CounterComponent());

            // Queryable by a capability interface it implements, not just its concrete type.
            Assert.IsNotNull(_entity.GetComponent<IUpdatable>());
        }

        [Test]
        public void Update_TicksOnlyUpdatableComponents()
        {
            _entity.AddComponent(new TagComponent());
            var counter = _entity.AddComponent(new CounterComponent());

            _entity.Update(0.016f);
            _entity.Update(0.016f);

            Assert.AreEqual(2, counter.Ticks);
        }

        [Test]
        public void RemoveComponent_RemovesAndDetaches()
        {
            var tag = _entity.AddComponent(new TagComponent());

            var removed = _entity.RemoveComponent<TagComponent>();

            Assert.IsTrue(removed);
            Assert.AreEqual(0, _entity.ComponentCount);
            Assert.IsNull(tag.Owner);
        }

        [Test]
        public void RemoveComponent_Missing_ReturnsFalse()
        {
            Assert.IsFalse(_entity.RemoveComponent<TagComponent>());
        }

        [Test]
        public void Update_AllowsAComponentToRemoveItself()
        {
            var selfRemoving = _entity.AddComponent(new SelfRemovingComponent());
            var counter = _entity.AddComponent(new CounterComponent());

            _entity.Update(0.016f); // selfRemoving removes itself mid-iteration

            Assert.AreEqual(1, selfRemoving.Ticks);
            Assert.AreEqual(1, counter.Ticks, "the component after the self-removing one must still tick");
            Assert.IsFalse(_entity.HasComponent<SelfRemovingComponent>());
        }
    }
}
