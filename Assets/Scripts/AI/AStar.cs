using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarData
{
    public float f;
    public int count;
    public WorldTile tile;

    public AStarData(float _f, int _c, WorldTile _tile)
    {
        f = _f; count = _c; tile = _tile;
    }
}

public static class AStar
{
    public static Dictionary<WorldTile, float> g = new Dictionary<WorldTile, float>();
    public static Dictionary<WorldTile, float> f = new Dictionary<WorldTile, float>();

    public static Queue<AStarData> openSet = new Queue<AStarData>();
    public static Dictionary<WorldTile, WorldTile> cameFrom = new Dictionary<WorldTile, WorldTile>();
    public static List<WorldTile> GetPath(List<WorldTile> map, WorldTile start, WorldTile end)
    {
        Reset(map);

        int count = 0;
        openSet.Enqueue(new AStarData(0, count, start));
        g[start] = 0;
        f[start] = h(start, end);

        List<WorldTile> hash = new List<WorldTile>();

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue().tile;
            hash.Remove(current);

            if (current == end)
            {
                return ReconstructPath(end);
            }

            foreach (var n in current.GetWalkableNeighbors())
            {
                float tempG = g[current] + current.moveCost;

                if (tempG < g[n])
                {
                    if (cameFrom.ContainsKey(n))
                    {
                        cameFrom[n] = current;
                    }
                    else
                    {
                        cameFrom.Add(n, current);
                    }

                    g[n] = tempG;
                    f[n] = tempG + h(n, end);

                    if (!hash.Contains(n))
                    {
                        count++;
                        openSet.Enqueue(new AStarData(f[n], count, n));
                        hash.Add(n);
                    }
                }
            }
        }

        return null;
    }

    private static List<WorldTile> ReconstructPath(WorldTile end)
    {
        List<WorldTile> path = new List<WorldTile>();
        path.Insert(0, end);

        var current = end;
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }

    private static void Reset(List<WorldTile> map)
    {
        g.Clear();
        f.Clear();
        openSet.Clear();
        cameFrom.Clear();

        foreach (var t in map)
        {
            g.Add(t, float.MaxValue);
            f.Add(t, float.MaxValue);
        }
    }

    private static float h(WorldTile l, WorldTile r)
    {
        return Mathf.Abs(l.gridX - r.gridX) + Mathf.Abs(l.gridY - r.gridY);
    }
}
