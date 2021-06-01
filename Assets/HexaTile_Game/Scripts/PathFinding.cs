using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexaGridGame
{

    public class PathFinding : MonoBehaviour
    {
        TileGameManager Manager;

        private void Start()
        {
            Manager = GetComponent<TileGameManager>();
        }

        public void FindPath()
        {
            // Opened, Closed
            List<HexaTile> opened = new List<HexaTile>();
            HashSet<HexaTile> closed = new HashSet<HexaTile>();
        }
    }
}
