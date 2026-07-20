using UnityEngine;

namespace DesignPatterns.Factory.Sample
{
    /// <summary>
    /// The product family: a map tile. Client code (pathfinding, rendering,
    /// gameplay) works against this abstraction; the factory decides which
    /// concrete tile a given map symbol becomes.
    /// </summary>
    public abstract class Tile
    {
        public char Symbol { get; }
        public string DisplayName { get; }
        public bool IsWalkable { get; }
        public int MoveCost { get; }
        public Vector2Int Position { get; }

        protected Tile(char symbol, string displayName, bool isWalkable, int moveCost, Vector2Int position)
        {
            Symbol = symbol;
            DisplayName = displayName;
            IsWalkable = isWalkable;
            MoveCost = moveCost;
            Position = position;
        }

        public override string ToString() => $"{DisplayName} '{Symbol}' at ({Position.x},{Position.y})";
    }
}
