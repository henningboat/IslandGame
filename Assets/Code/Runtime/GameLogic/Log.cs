using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace IslandGame.GameLogic
{
    public class Log : Interaction
    {
        [SerializeField] private AudioSource _audioSource;

        protected override IEnumerator Interact(PlayerInventory playerInventory)
        {
            transform.DOScale(0, 0.1f);
            _audioSource.Play();
            yield return transform.DOMove(playerInventory.transform.position, 0.1f);
            playerInventory.AddLog();
        }
    }
}