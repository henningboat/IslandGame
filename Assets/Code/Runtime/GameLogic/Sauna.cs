using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace IslandGame.GameLogic
{
    public class Sauna : Interaction
    {
       [SerializeField] private CanvasGroup _noLogsCanvas;
       [SerializeField] private CanvasGroup _aSaunaCanvas;
       [SerializeField] private int _requiredLogs = 3;

       protected override IEnumerator Interact(PlayerInventory playerInventory)
       {
           if (playerInventory.LogCount >= _requiredLogs)
           {
               yield return _aSaunaCanvas.DOFade(1, 0.5f).WaitForCompletion();
               yield return new WaitForSeconds(3);
               yield return _aSaunaCanvas.DOFade(0, 0.5f).WaitForCompletion();
           }
           else
           {
               yield return _noLogsCanvas.DOFade(1, 0.5f).WaitForCompletion();
               yield return new WaitForSeconds(3);
               yield return _noLogsCanvas.DOFade(0, 0.5f).WaitForCompletion();
           }
       }
    }
}
