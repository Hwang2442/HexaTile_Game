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

        private void Start()
        {
            //transform.position += Vector3.up * 0.2f;

            Debug.Log("start");
        }

        public void Move(HexaTile tile)
        {
            Moving = true;

            StartCoroutine(Co_Move(tile));
        }

        IEnumerator Co_Move(HexaTile tile)
        {
            float t = 0;

            Vector3 start = transform.position;
            Vector3 target = tile.transform.position;
            target.y += 0.2f;

            while (t < 1)
            {
                t += Time.deltaTime;

                transform.position = Vector3.Lerp(start, target, t / 0.2f);

                yield return null;
            }

            Tile = tile;

            Moving = false;

            Manager.PlayerOnClearTile(Tile);
        }
    }
}

