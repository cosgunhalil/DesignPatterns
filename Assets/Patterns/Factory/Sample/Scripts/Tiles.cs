using UnityEngine;

namespace DesignPatterns.Factory.Sample
{
    /// <summary>Open ground: cheap to cross.</summary>
    public sealed class FloorTile : Tile
    {
        public FloorTile(Vector2Int position) : base('.', "Floor", true, 1, position)
        {
        }
    }

    /// <summary>Solid wall: blocks movement.</summary>
    public sealed class WallTile : Tile
    {
        public WallTile(Vector2Int position) : base('#', "Wall", false, 0, position)
        {
        }
    }

    /// <summary>Water: passable but slow.</summary>
    public sealed class WaterTile : Tile
    {
        public WaterTile(Vector2Int position) : base('~', "Water", true, 3, position)
        {
        }
    }

    /// <summary>Door: passable at a small cost.</summary>
    public sealed class DoorTile : Tile
    {
        public DoorTile(Vector2Int position) : base('+', "Door", true, 2, position)
        {
        }
    }

    /// <summary>
    /// A hazard tile the demo registers at runtime to show the factory being
    /// extended without editing it — <see cref="TileFactory"/> never mentions this type.
    /// </summary>
    public sealed class LavaTile : Tile
    {
        public LavaTile(Vector2Int position) : base('L', "Lava", false, 0, position)
        {
        }
    }
}
