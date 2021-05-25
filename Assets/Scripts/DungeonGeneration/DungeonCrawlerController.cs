using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Direction
{
    up = 0,
    left = 1,
    down = 2,
    right = 3
}
public class DungeonCrawlerController : MonoBehaviour
{
    public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
    private static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>
    {
        {Direction.up, Vector2Int.up},
        {Direction.left, Vector2Int.left},
        {Direction.down, Vector2Int.down},
        {Direction.right, Vector2Int.right}
    };

    public static List<Vector2Int> GenerateDungeon(DungeonGenerationData dungeonData)
    {
        var dungeonCrawlers = new List<DungeonCrawler>();
        for (var i = 0; i < dungeonData.numberOfCrawlers; i++)
        {
            dungeonCrawlers.Add(new DungeonCrawler(Vector2Int.zero));
        }

        var iterations = Random.Range(dungeonData.iterationMin, dungeonData.iterationMax);

        for (var i = 0; i < iterations; i++)
        {
            foreach (var newPosition in dungeonCrawlers.Select(dungeonCrawler => dungeonCrawler.Move(directionMovementMap)))
            {
                positionsVisited.Add(newPosition);
            }
        }

        return positionsVisited;
    }
}
