using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    public class HexaTile : MonoBehaviour
    {
        [SerializeField]
        bool isWall = false;

        public bool IsWall 
        { 
            get
            {
                return isWall;
            }
            set
            {
                isWall = value;

                ShowWall(isWall);
            } 
        }

        MeshRenderer renderer;
        public MeshRenderer Renderer 
        {
            get
            {
                if (renderer == null)
                {
                    renderer = GetComponent<MeshRenderer>();
                }

                return renderer;
            }
        }

        public static TileGameManager Manager { get; set; }
        public static bool IsTouch { get; set; }

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

        private void OnMouseUpAsButton()
        {
            if (Manager != null && IsTouch)
            {
                Manager.OnTileClicked(this);
            }
        }

        public void ShowWall(bool active)
        {
            transform.GetChild(transform.childCount - 1).gameObject.SetActive(active);
        }
    }
}
