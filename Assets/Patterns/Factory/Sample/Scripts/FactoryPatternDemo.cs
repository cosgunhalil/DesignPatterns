using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DesignPatterns.Factory.Sample
{
    /// <summary>
    /// Entry point. Press Play — it parses an ASCII map, lets the factory turn
    /// each symbol into the right Tile, prints the rebuilt map and some stats,
    /// then extends the factory with a brand-new tile type at runtime and shows
    /// an unknown symbol handled without an exception.
    /// </summary>
    public sealed class FactoryPatternDemo : MonoBehaviour
    {
        private static readonly string[] Map =
        {
            "#####",
            "#.+.#",
            "#.~.#",
            "#####"
        };

        private void Start()
        {
            var factory = new TileFactory();
            var tiles = BuildTiles(factory, Map);

            LogParsedMap(tiles, Map[0].Length);

            var walkable = 0;
            var totalCost = 0;
            foreach (var tile in tiles)
            {
                if (tile.IsWalkable)
                {
                    walkable++;
                    totalCost += tile.MoveCost;
                }
            }

            Debug.Log($"<color=cyan>{walkable} walkable tiles, total move cost {totalCost}.</color>");

            // Extend at runtime — no edit to TileFactory or KeyedFactory.
            factory.RegisterTile('L', position => new LavaTile(position));
            var lava = factory.CreateTile('L', new Vector2Int(2, 2));
            Debug.Log($"<color=orange>Extended factory:</color> {lava} (walkable={lava.IsWalkable})");

            // Unknown symbol handled gracefully via TryCreate.
            if (!factory.TryCreateTile('?', Vector2Int.zero, out _))
            {
                Debug.Log("<color=grey>Unknown symbol '?' → no tile created (TryCreateTile returned false).</color>");
            }
        }

        private static Tile[] BuildTiles(TileFactory factory, string[] map)
        {
            var tiles = new List<Tile>();
            for (var y = 0; y < map.Length; y++)
            {
                var row = map[y];
                for (var x = 0; x < row.Length; x++)
                {
                    tiles.Add(factory.CreateTile(row[x], new Vector2Int(x, y)));
                }
            }

            return tiles.ToArray();
        }

        private static void LogParsedMap(IReadOnlyList<Tile> tiles, int width)
        {
            var sb = new StringBuilder("Parsed map from tiles:\n");
            for (var i = 0; i < tiles.Count; i++)
            {
                sb.Append(tiles[i].Symbol);
                if ((i + 1) % width == 0)
                {
                    sb.Append('\n');
                }
            }

            Debug.Log(sb.ToString());
        }
    }
}
