using UnityEngine;

namespace IslandGame.GameLogic
{
    public class PlayerInventory : MonoBehaviour
    {
        public int LogCount;

        public void AddLog()
        {
            LogCount++;
        }

        public void ConsumeLogs(int logs)
        {
            LogCount -= logs;
        }
    }
}