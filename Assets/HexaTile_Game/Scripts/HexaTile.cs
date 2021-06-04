using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    public class HexaTile : MonoBehaviour
    {
        public bool isWall = false;

        public SpriteRenderer Renderer { get; private set; }

        public static TileGameManager Manager { get; set; }

        public int IndexX { get; set; }
        public int IndexY { get; set; }

        public int TotalCost { get { return CostFromStart + CostToGoal; } } // fCost
        public int CostFromStart { get; set; }  // gCost
        public int CostToGoal { get; set; }     // hCost

        public HexaTile Parent { get; set; }

        public List<HexaTile> Neighbours
        {
            get
            {
                List<HexaTile> neighbours = new List<HexaTile>();

                bool even = (IndexY % 2 == 0);

                // Clockwise { RightUp, Right, RightDown, LeftDown, Left, LeftUp }
                int[,] direction = { { -1, even ? 0 : 1 }, { 0, 1 }, { 1, even ? 0 : 1 }, { 1, even ? -1 : 0 }, { 0, -1 }, { -1, even ? -1 : 0 } };

                for (int i = 0; i < 6; i++)
                {
                    int x = IndexX + direction[i, 1];
                    int y = IndexY + direction[i, 0];

                    if (x >= 0 && y >= 0 && x < Manager.grid.x && y < Manager.grid.y)
                    {
                        neighbours.Add(Manager.tiles[y, x]);
                    }
                }

                return neighbours;
            }
        }

        private void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnMouseUpAsButton()
        {
            if (Manager != null)
            {
                Manager.OnTileClicked(this);
            }
        }
    }
}
