using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

namespace HexaGridGame
{
    public class EscapeMan : MonoBehaviour
    {
        private Animator animator;

        public HexaTile Tile { get; set; }

        public bool Moving { get; private set; }

        public TileGameManager Manager { get; set; }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        private void OnDestroy()
        {
            transform.DOKill();
        }

        public void Move(HexaTile tile)
        {
            Moving = true;
            animator.SetTrigger("Move");
            transform.LookAt(tile.transform);

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOMove(tile.transform.position, 0.667f).SetEase(Ease.InOutSine));
            sequence.AppendCallback(() => Moving = false);
        }
    }
}

