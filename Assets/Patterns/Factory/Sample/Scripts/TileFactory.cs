using System;
using UnityEngine;

namespace DesignPatterns.Factory.Sample
{
    /// <summary>
    /// Turns a map symbol (+ grid position) into the right <see cref="Tile"/>.
    /// It composes the generic <see cref="KeyedFactory{TKey,TArg,TProduct}"/> and
    /// registers the built-in symbols; callers can register more via
    /// <see cref="RegisterTile"/> without this class changing.
    /// </summary>
    public sealed class TileFactory
    {
        private readonly KeyedFactory<char, Vector2Int, Tile> _factory;

        public TileFactory()
        {
            _factory = new KeyedFactory<char, Vector2Int, Tile>();
            _factory.Register('.', position => new FloorTile(position)); // lambda creators
            _factory.Register('#', position => new WallTile(position));
            _factory.Register('~', position => new WaterTile(position));
            _factory.Register('+', new DoorTileFactory());               // object-based creator
        }

        public Tile CreateTile(char symbol, Vector2Int position) => _factory.Create(symbol, position);

        public bool TryCreateTile(char symbol, Vector2Int position, out Tile tile) =>
            _factory.TryCreate(symbol, position, out tile);

        public bool CanCreate(char symbol) => _factory.CanCreate(symbol);

        /// <summary>Register a new tile symbol at runtime — the open/closed payoff of a registry.</summary>
        public void RegisterTile(char symbol, Func<Vector2Int, Tile> creator) => _factory.Register(symbol, creator);
    }
}
