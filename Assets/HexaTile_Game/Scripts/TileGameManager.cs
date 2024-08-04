using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using DG.Tweening;

namespace HexaGridGame
{
    public class TileGameManager : MonoBehaviour
    {
        private const float xInterval = 1.905354f;
        private const float zInterval = 1.65f;

        [SerializeField] private HexaTile[] tilePrefabs;
        [SerializeField] private GameObject[] wallPrefabs;

        // Tiles ang Grid
        public HexaTile[,] tiles;
        public Vector2Int grid;

        // Escape Player
        public EscapeMan player;

        public int wallNum;

        PathFinding pathFinding;

        [SerializeField] List<HexaTile> escapeTiles = new List<HexaTile>();

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
            player.transform.position = player.Tile.transform.position/* + Vector3.up * 0.2f*/;

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
                    //tile.Renderer.color = Color.black;
                }
            }
        }

        public void OnTileClicked(HexaTile tile)
        {
            Debug.Log("Clicked Tile : " + tile.gameObject.name);

            if (tile == player.Tile || player.Moving) return;

            // Tile is Obstacle
            tile.IsWall = true;
            //tile.Renderer.color = Color.black;
            GameObject wallObj = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)], transform);
            wallObj.transform.position += tile.transform.position;
            wallObj.transform.DOMoveY(wallObj.transform.position.y, 0.667f).From(1).SetEase(Ease.OutCirc);

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
                Vector3 startPosition = new Vector3(Mathf.CeilToInt(grid.x * -1 * 0.5f), 0, Mathf.FloorToInt(grid.y * 0.5f));

                tiles = new HexaTile[grid.y, grid.x];

                for (int i = 0; i < grid.y; i++)
                {
                    bool even = (i % 2 == 0);

                    float x = even ? startPosition.x : startPosition.x + xInterval * 0.5f;
                    float z = startPosition.z - i;

                    for (int j = 0; j < grid.x; j++)
                    {
                        var target = tilePrefabs[Random.Range(0, tilePrefabs.Length)];

                        var tile = Instantiate(target, transform);
                        tile.transform.localPosition = new Vector3(x + j * xInterval, 0, z * zInterval);
                        tile.transform.localRotation = Quaternion.Euler(0, 60 * Random.Range(0, 6), 0);

                        string name = "_" + i.ToString() + "_" + j.ToString();
                        tile.transform.name += name;

                        tile.IndexX = j;
                        tile.IndexY = i;

                        if (i == 0 || i == grid.y - 1)
                            escapeTiles.Add(tile);
                        else
                        {
                            if (j == 0 || j == grid.x - 1)
                                escapeTiles.Add(tile);
                        }

                        tiles[i, j] = tile;
                    }
                }
            }
        }
    }
}
