using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{

    public class PathFinding : MonoBehaviour
    {
        public List<HexaTile> path = new List<HexaTile>();

        TileGameManager Manager;

        private void Start()
        {
            Manager = GetComponent<TileGameManager>();
        }

        public bool FindPath(HexaTile currentTile, HexaTile target)
        {
            path.Clear();

            // Opened, Closed
            List<HexaTile> opened = new List<HexaTile>();
            HashSet<HexaTile> closed = new HashSet<HexaTile>();

            HexaTile start = currentTile;

            opened.Add(Manager.player.Tile);

            while (opened.Count > 0)
            {
                foreach (HexaTile neighbour in currentTile.Neighbours)
                {
                    if (!neighbour.IsWall && !closed.Contains(neighbour))
                    {
                        int cost = currentTile.CostFromStart + 1;

                        if (!opened.Contains(neighbour) || cost < neighbour.CostFromStart)
                        {
                            neighbour.CostFromStart = cost;
                            neighbour.CostToGoal = GetDistance(neighbour, target);
                            neighbour.Parent = currentTile;

                            if (!opened.Contains(neighbour))
                            {
                                opened.Add(neighbour);
                            }
                        }
                    }
                }

                currentTile = opened[0];
                foreach (HexaTile openTile in opened)
                {
                    if (openTile.TotalCost < currentTile.TotalCost)
                    {
                        currentTile = openTile;
                    }
                    else if (openTile.TotalCost == currentTile.TotalCost)
                    {
                        if (openTile.CostToGoal < currentTile.CostToGoal)
                        {
                            currentTile = openTile;
                        }
                    }
                }

                opened.Remove(currentTile);
                closed.Add(currentTile);

                if (currentTile == target)
                {
                    while (currentTile != start)
                    {
                        //currentTile.Renderer.color = Color.green;
                        path.Add(currentTile);
                        currentTile = currentTile.Parent;
                    }

                    break;
                }
            }

            return (path.Count > 0);
        }

        int GetDistance(HexaTile from, HexaTile to)
        {
            int y = Mathf.Abs(to.IndexY - from.IndexY);
            int x = 0;

            int minX = 0, maxX = 0;

            int range = y / 2;

            if (y % 2 == 0)
            {
                minX = from.IndexX - range;
                maxX = from.IndexX + range;
            }
            else
            {
                // even
                if (from.IndexY % 2 == 0)
                {
                    minX = from.IndexX - (range + 1);
                    maxX = from.IndexX + range;
                }
                else
                {
                    minX = from.IndexX - range;
                    maxX = from.IndexX + (range + 1);
                }
            }

            minX = Mathf.Max(0, minX);
            maxX = Mathf.Min(Manager.grid.x - 1, maxX);

            if (to.IndexX < minX)
            {
                x = minX - to.IndexX;
            }
            else if (to.IndexX > maxX)
            {
                x = to.IndexX - maxX;
            }

            return x + y;
        }
    }
}
