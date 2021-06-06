using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    public class EscapeMan : MonoBehaviour
    {
        public HexaTile Tile { get; set; }

        public bool Moving { get; private set; }

        public TileGameManager Manager { get; set; }

        public void Move(HexaTile tile)
        {
            Moving = true;

            StartCoroutine(Co_Move(tile));
        }

        IEnumerator Co_Move(HexaTile tile)
        {
            float t = 0;

            Vector3 start = transform.position;

            while (t < 1)
            {
                t += Time.deltaTime;

                transform.position = Vector3.Lerp(start, tile.transform.position, t / 0.3f);

                yield return null;
            }

            transform.position = tile.transform.position;

            Tile = tile;

            Moving = false;

            Manager.PlayerOnClearTile(Tile);
        }
    }
}

