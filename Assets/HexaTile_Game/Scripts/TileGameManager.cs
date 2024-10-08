﻿using System.Linq;
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

        [SerializeField] private int stepCount = 0;

        [Header("Setting")]
        [SerializeField] private TileManagerSettingTable settingTable;
        [SerializeField] private TileManagerSettingTable.Level selectedLevel;
        [SerializeField] private ParticleSystem wallParticle;
        [SerializeField] private EscapeMan player;

        [Header("Tile")]
        [SerializeField] private Vector2Int grid;
        [SerializeField] private HexaTile[,] tiles;
        [SerializeField] List<HexaTile> escapeTiles = new List<HexaTile>();


        private PathFinding pathFinding;

        [Space]
        [SerializeField] private UnityEvent onVictory;
        [SerializeField] private UnityEvent onDefeat;

        public Camera MainCam { get; private set; }
        public HexaTile[,] Tiles => tiles;
        public Vector2Int Grid => grid;
        public EscapeMan Player => player;
        public int StepCount => stepCount;

        private void Start()
        {
            SoundManager.Instance.Play("BGM", 0.5f);

            stepCount = 0;
            HexaTile.Manager = this;
            HexaTile.IsTouch = true;
            MainCam = Camera.main;
            pathFinding = GetComponent<PathFinding>();

            player.Manager = this;
            player.gameObject.SetActive(false);
            CreateTile(() =>
            {
                // Player Setting
                player.Tile = tiles[grid.y / 2, grid.x / 2];
                Vector3 pos = player.Tile.transform.position;
                player.transform.position = pos;
                player.gameObject.SetActive(true);

                if (tiles.Length > selectedLevel.StartWallCount)
                {
                    for (int i = 0; i < selectedLevel.StartWallCount; i++)
                    {
                        Vector2Int index = new Vector2Int(Random.Range(0, grid.x), Random.Range(0, grid.y));

                        HexaTile tile = tiles[index.y, index.x];

                        if (tile.IsWall || tile == player.Tile)
                        {
                            i--;
                            continue;
                        }

                        tile.IsWall = true;
                        GameObject wallObj = Instantiate(settingTable.GetRandomWall(), transform);
                        tile.SetWall(wallObj, wallParticle, false);
                    }
                }
            });
        }

        public void SetLevel(int index)
        {
            selectedLevel = settingTable.GetLevel(index);
            if (selectedLevel == null)
                selectedLevel = settingTable.GetLevel(0);
        }

        public void OnTileClicked(HexaTile tile)
        {
            Debug.Log("Clicked Tile : " + tile.gameObject.name);

            if (tile == player.Tile || player.Moving) return;

            // Tile is Obstacle
            tile.IsWall = true;
            GameObject wallObj = Instantiate(settingTable.GetRandomWall(), transform);
            tile.SetWall(wallObj, wallParticle);

            ++stepCount;

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
                SoundManager.Instance.PlayOneShot("Jump");
            }
        }



        public void PlayerOnClearTile(HexaTile tile)
        {
            if (escapeTiles.Contains(tile))
            {
                onDefeat.Invoke();
            }
        }

        void CreateTile(UnityAction onComplete)
        {
            if (grid.x <= 0 || grid.y <= 0)
                return;

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
                    var target = settingTable.GetRandomTile();

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

                    // Create tile animation
                    var tween = tile.transform.DOLocalMoveY(0, 0.2f).From(-4).SetDelay((i + j) * 0.1f).SetEase(Ease.OutBack).Pause();
                    tween.OnStart(() => SoundManager.Instance.PlayOneShot("CreateTile"));
                    if (i == grid.y - 1 && j == grid.x - 1)
                        tween.onComplete += onComplete.Invoke;

                    tween.Play();
                }
            }

            foreach (var escapeTile in escapeTiles)
            {
                var fx = Instantiate(settingTable.GetFailTileFX(), escapeTile.transform);
                fx.transform.localPosition = new Vector3(0, 0.01f, 0);
                fx.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
