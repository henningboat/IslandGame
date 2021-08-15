using System.Collections;
using DG.Tweening;

namespace IslandGame.GameLogic
{
    public class Log : Interaction
    {
        protected override IEnumerator Interact(PlayerInventory playerInventory)
        {
            transform.DOScale(0, 0.1f);
            yield return transform.DOMove(playerInventory.transform.position, 0.1f);
            playerInventory.AddLog();
        }
    }
}
