using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    public class TileGameManager : MonoBehaviour
    {
        public HexaTile tilePrefab;

        public HexaTile[,] tiles;

        public Vector2Int grid;

        public Camera MainCam { get; private set; }

        private void Start()
        {
            HexaTile.Manager = this;

            MainCam = Camera.main;

            CreateTile();
        }

        public void OnTileClicked(HexaTile tile)
        {
            Debug.Log("Clicked Tile : " + tile.gameObject.name);

            tile.isWall = true;

            tile.Renderer.color = Color.black;
        }

        void CreateTile()
        {
            if (grid.x > 0 && grid.y > 0)
            {
                // Setup LeftTop Tile position
                Vector2 startPosition = new Vector2(Mathf.CeilToInt(grid.x * -1 * 0.5f), Mathf.FloorToInt(grid.y * 0.5f));

                tiles = new HexaTile[grid.y, grid.x];

                for (int i = 0; i < grid.y; i++)
                {
                    bool even = (i % 2 == 0);

                    float x = even ? startPosition.x : startPosition.x + 0.5f;
                    float y = startPosition.y - i;

                    for (int j = 0; j < grid.x; j++)
                    {
                        tiles[i, j] = Instantiate(tilePrefab, transform);
                        tiles[i, j].transform.position = new Vector3(x + j, y);

                        string name = "_" + i.ToString() + "_" + j.ToString();
                        tiles[i, j].transform.name += name;
                    }
                }
            }
        }
    }
}
