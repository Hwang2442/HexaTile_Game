using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{

    public class PathFinding : MonoBehaviour
    {
        public bool escapeEnable;

        public List<HexaTile> path;

        TileGameManager Manager;

        private void Start()
        {
            Manager = GetComponent<TileGameManager>();
        }

        public void FindPath(HexaTile currentTile, HexaTile target)
        {
            // Opened, Closed
            List<HexaTile> opened = new List<HexaTile>();
            HashSet<HexaTile> closed = new HashSet<HexaTile>();

            HexaTile start = currentTile;

            opened.Add(Manager.player.Tile);

            while (opened.Count > 0)
            {
                foreach (HexaTile neighbour in currentTile.Neighbours)
                {
                    if (!neighbour.isWall && !closed.Contains(neighbour))
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

                opened.Remove(currentTile);
                closed.Add(currentTile);

                if (currentTile == target)
                {
                    escapeEnable = true;

                    while (currentTile != start)
                    {
                        path.Add(currentTile);
                        currentTile.Renderer.color = Color.green;
                        currentTile = currentTile.Parent;
                    }

                    break;
                }

                currentTile = opened[0];

                if (currentTile == null) break;

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
            }
        }

        int GetDistance(HexaTile from, HexaTile to)
        {
            bool evenFrom = (from.IndexY % 2) == 0;
            bool evenTo = (to.IndexY % 2) == 0;

            int y = Mathf.Abs(to.IndexY - from.IndexY);
            int x = Mathf.Abs(to.IndexX - from.IndexX);

            x = Mathf.Max(0, x - 2);

            return x + y;
        }
    }
}
