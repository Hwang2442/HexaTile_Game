using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace HexaGridGame
{
    public class HexaTile : MonoBehaviour
    {
        [SerializeField]
        bool isWall = false;

        public bool IsWall 
        { 
            get
            {
                return isWall;
            }
            set
            {
                isWall = value;
            } 
        }

        MeshRenderer renderer;
        public MeshRenderer Renderer 
        {
            get
            {
                if (renderer == null)
                {
                    renderer = GetComponent<MeshRenderer>();
                }

                return renderer;
            }
        }

        public static TileGameManager Manager { get; set; }
        public static bool IsTouch { get; set; }

        public int IndexX { get; set; }
        public int IndexY { get; set; }

        public int TotalCost { get { return CostFromStart + CostToGoal; } } // fCost
        public int CostFromStart { get; set; }  // gCost
        public int CostToGoal { get; set; }     // hCost

        public HexaTile Parent { get; set; }

        public List<HexaTile> Neighbours
        {
            get
            {
                List<HexaTile> neighbours = new List<HexaTile>();

                bool even = (IndexY % 2 == 0);

                // Clockwise { RightUp, Right, RightDown, LeftDown, Left, LeftUp }
                int[,] direction = { { -1, even ? 0 : 1 }, { 0, 1 }, { 1, even ? 0 : 1 }, { 1, even ? -1 : 0 }, { 0, -1 }, { -1, even ? -1 : 0 } };

                for (int i = 0; i < 6; i++)
                {
                    int x = IndexX + direction[i, 1];
                    int y = IndexY + direction[i, 0];

                    if (x >= 0 && y >= 0 && x < Manager.grid.x && y < Manager.grid.y)
                    {
                        neighbours.Add(Manager.tiles[y, x]);
                    }
                }

                return neighbours;
            }
        }

        private void OnMouseUpAsButton()
        {
            if (Manager != null && IsTouch)
            {
                Manager.OnTileClicked(this);
            }
        }

        public void SetWall(GameObject wallObj, ParticleSystem particle, bool useAnimation = true)
        {
            wallObj.transform.position += transform.position;
            if (useAnimation)
            {
                wallObj.transform.DOScale(wallObj.transform.localScale, 0.1f).From(0).SetEase(Ease.OutCirc).OnComplete(() =>
                {
                    SoundManager.Instance.PlayOneShot("Wall");
                });
            }
            else
            {
                wallObj.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
                SoundManager.Instance.PlayOneShot("Wall");
            }

            if (particle.isPlaying)
            {
                particle = Instantiate(particle, transform.position, Quaternion.identity);
                var main = particle.main;
                main.stopAction = ParticleSystemStopAction.Destroy;
            }
            particle.transform.position = transform.position;
            particle.Play();
        }
    }
}
