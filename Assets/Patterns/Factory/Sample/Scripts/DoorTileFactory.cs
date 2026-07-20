using UnityEngine;

namespace DesignPatterns.Factory.Sample
{
    /// <summary>
    /// A creator expressed as a dedicated factory OBJECT rather than a lambda,
    /// registered through the KeyedFactory's <c>IFactory</c> overload. Reach for
    /// this style when creation needs its own state or injected dependencies;
    /// reach for a lambda when it's a one-liner.
    /// </summary>
    public sealed class DoorTileFactory : IFactory<Vector2Int, Tile>
    {
        public Tile Create(Vector2Int position) => new DoorTile(position);
    }
}
