using NUnit.Framework;
using UnityEngine;
using DesignPatterns.Factory.Sample;

namespace DesignPatterns.Factory.Tests
{
    public class TileFactoryTests
    {
        private TileFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new TileFactory();
        }

        [Test]
        public void Floor_IsWalkableWithCostOne()
        {
            var tile = _factory.CreateTile('.', new Vector2Int(1, 2));

            Assert.IsInstanceOf<FloorTile>(tile);
            Assert.IsTrue(tile.IsWalkable);
            Assert.AreEqual(1, tile.MoveCost);
            Assert.AreEqual(new Vector2Int(1, 2), tile.Position);
        }

        [Test]
        public void Wall_IsNotWalkable()
        {
            var tile = _factory.CreateTile('#', Vector2Int.zero);

            Assert.IsInstanceOf<WallTile>(tile);
            Assert.IsFalse(tile.IsWalkable);
        }

        [Test]
        public void Water_IsWalkableButCostly()
        {
            var tile = _factory.CreateTile('~', Vector2Int.zero);

            Assert.IsInstanceOf<WaterTile>(tile);
            Assert.AreEqual(3, tile.MoveCost);
        }

        [Test]
        public void Door_CreatedViaObjectFactory()
        {
            var tile = _factory.CreateTile('+', Vector2Int.zero);

            Assert.IsInstanceOf<DoorTile>(tile);
            Assert.AreEqual(2, tile.MoveCost);
        }

        [Test]
        public void UnknownSymbol_TryCreateReturnsFalse()
        {
            Assert.IsFalse(_factory.TryCreateTile('?', Vector2Int.zero, out var tile));
            Assert.IsNull(tile);
        }

        [Test]
        public void RegisterTile_ExtendsTheFactoryAtRuntime()
        {
            Assert.IsFalse(_factory.CanCreate('L'));

            _factory.RegisterTile('L', position => new LavaTile(position));

            Assert.IsTrue(_factory.CanCreate('L'));
            Assert.IsInstanceOf<LavaTile>(_factory.CreateTile('L', Vector2Int.zero));
        }

        [Test]
        public void EachCreate_ProducesAFreshInstance()
        {
            var a = _factory.CreateTile('.', Vector2Int.zero);
            var b = _factory.CreateTile('.', Vector2Int.zero);

            Assert.AreNotSame(a, b);
        }

        [Test]
        public void PositionIsPropagatedToTheProduct()
        {
            var tile = _factory.CreateTile('~', new Vector2Int(5, 7));

            Assert.AreEqual(new Vector2Int(5, 7), tile.Position);
        }
    }
}
