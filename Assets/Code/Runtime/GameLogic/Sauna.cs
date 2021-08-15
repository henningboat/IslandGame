using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IslandGame.GameLogic
{
    public class Sauna : Interaction
    {
        [SerializeField] private CanvasGroup _noLogsCanvas;
        [SerializeField] private AudioSource _noLogsAudio;

        [SerializeField] private CanvasGroup _aSaunaCanvas;
        [SerializeField] private int _requiredLogs = 3;
        [SerializeField] private AudioSource _saunaAudio;

        protected override IEnumerator Interact(PlayerInventory playerInventory)
        {
            if (playerInventory.LogCount >= _requiredLogs)
            {
                playerInventory.GetComponent<Player>().enabled = false;

                _saunaAudio.Play();
                yield return _aSaunaCanvas.DOFade(1, 0.5f).WaitForCompletion();
                yield return new WaitForSeconds(2);
                yield return _aSaunaCanvas.DOFade(0, 0.5f).WaitForCompletion();

                SceneManager.LoadScene(0);
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