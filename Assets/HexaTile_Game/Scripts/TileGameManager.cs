﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

namespace HexaGridGame
{
    public class TileGameManager : MonoBehaviour
    {
        [SerializeField]
        HexaTile tilePrefab;

        // Tiles ang Grid
        public HexaTile[,] tiles;
        public Vector2Int grid;

        // Escape Player
        public EscapeMan player;

        public int wallNum;

        PathFinding pathFinding;

        [SerializeField]
        List<HexaTile> escapeTiles = new List<HexaTile>();

        [Space]
        public UnityEvent onVictory;
        public UnityEvent onDefeat;

        public Camera MainCam { get; private set; }

        private void Start()
        {
            HexaTile.Manager = this;
            HexaTile.IsTouch = true;
            MainCam = Camera.main;
            pathFinding = GetComponent<PathFinding>();

            CreateTile();

            // Player Setting
            player.Manager = this;
            player.Tile = tiles[grid.y / 2, grid.x / 2];
            player.transform.position = player.Tile.transform.position + Vector3.up * 0.2f;

            if (tiles.Length > wallNum)
            {
                for (int i = 0; i < wallNum; i++)
                {
                    Vector2Int index = new Vector2Int(Random.Range(0, grid.x), Random.Range(0, grid.y));

                    HexaTile tile = tiles[index.y, index.x];

                    if (tile.IsWall || tile == player.Tile)
                    {
                        i--;
                        continue;
                    }

                    tile.IsWall = true;
                    tile.Renderer.color = Color.black;
                }
            }
        }

        public void OnTileClicked(HexaTile tile)
        {
            Debug.Log("Clicked Tile : " + tile.gameObject.name);

            if (tile == player.Tile || player.Moving) return;

            // Tile is Obstacle
            tile.IsWall = true;
            tile.Renderer.color = Color.black;

            // FindPath
            int count = 999;
            HexaTile nextTile = null;
            var negibours = player.Tile.Neighbours;

            bool isLock = true;
            for (int i = 0; i < negibours.Count; i++)
            {
                if (!negibours[i].IsWall)
                {
                    isLock = false;

                    break;
                }
            }

            // Victory!!
            if (isLock)
            {
                onVictory.Invoke();

                return;
            }

            do
            {
                int random = Random.Range(0, negibours.Count);

                if (!negibours[random].IsWall)
                {
                    nextTile = negibours[random];
                }

            } while (nextTile == null);

            if (nextTile != null)
            {
                for (int i = 0; i < escapeTiles.Count; i++)
                {
                    if (!escapeTiles[i].IsWall)
                    {
                        if (pathFinding.FindPath(player.Tile, escapeTiles[i]))
                        {
                            if (count > pathFinding.path.Count)
                            {
                                count = pathFinding.path.Count;
                                nextTile = pathFinding.path.Last();
                            }
                        }
                    }
                }

                player.Tile = nextTile;

                player.Move(nextTile);
            }
        }



        public void PlayerOnClearTile(HexaTile tile)
        {
            if (escapeTiles.Contains(tile))
            {
                onDefeat.Invoke();
            }
        }

        void CreateTile()
        {
            if (grid.x > 0 && grid.y > 0)
            {
                grid.x = Mathf.Clamp(grid.x, 3, 9);
                grid.y = Mathf.Clamp(grid.y, 3, 9);

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

                        tiles[i, j].IndexX = j;
                        tiles[i, j].IndexY = i;

                        if (i == 0 || i == grid.y - 1)
                        {
                            escapeTiles.Add(tiles[i, j]);
                        }
                        else
                        {
                            if (j == 0 || j == grid.x - 1)
                            {
                                escapeTiles.Add(tiles[i, j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
