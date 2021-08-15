using TMPro;
using UnityEngine;

namespace IslandGame.GameLogic
{
    public class LogUI : MonoBehaviour
    {
        private PlayerInventory _playerInventory;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _playerInventory = GetComponentInParent<PlayerInventory>();
        }

        private void Update()
        {
            _text.text = $"{_playerInventory.LogCount}/3";
        }
    }
}