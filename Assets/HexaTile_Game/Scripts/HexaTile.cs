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

        private void Start()
        {
            Renderer = GetComponent<SpriteRenderer>();
        }

        private void OnMouseDown()
        {
            if (Manager != null)
            {
                Manager.OnTileClicked(this);
            }
        }
    }
}
