using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    [CreateAssetMenu(fileName = "ManagerSettingTable", menuName = "HexaTile/ManagerSettingTable")]
    public class TileManagerSettingTable : ScriptableObject
    {
        [Header("Tile")]
        [SerializeField] private HexaTile[] tilePrefabs;
        [SerializeField] private ParticleSystem failTileFX;

        [Header("Wall")]
        [SerializeField] private GameObject[] wallPrefabs;

        #region Tile

        public HexaTile GetRandomTile()
        {
            return GetTile(Random.Range(0, tilePrefabs.Length));
        }

        public HexaTile GetTile(int index)
        {
            if (index != Mathf.Clamp(index, 0, tilePrefabs.Length - 1)) 
                return null;

            return tilePrefabs[index];
        }

        #endregion

        #region Wall

        public GameObject GetRandomWall()
        {
            return GetWall(Random.Range(0, wallPrefabs.Length));
        }

        public GameObject GetWall(int index)
        {
            if (index != Mathf.Clamp(index, 0, wallPrefabs.Length - 1))
                return null;

            return wallPrefabs[index];
        }

        #endregion

        public ParticleSystem GetFailTileFX()
        {
            return failTileFX;
        }
    }
}