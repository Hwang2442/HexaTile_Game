using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{
    public class EscapeMan : MonoBehaviour
    {
        public HexaTile Tile { get; set; }

        public void Move()
        {
            StartCoroutine(Co_Move());
        }

        IEnumerator Co_Move()
        {
            float t = 0;

            Vector3 start = transform.position;

            while (t < 1)
            {
                t += Time.deltaTime;

                transform.position = Vector3.Lerp(start, Tile.transform.position, t / 0.5f);

                yield return null;
            }
        }
    }
}

