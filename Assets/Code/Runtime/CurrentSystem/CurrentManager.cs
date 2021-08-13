using IslandGame.Utils;
using UnityEngine;

namespace IslandGame.CurrentSystem
{
    public class CurrentManager : Singleton<CurrentManager>
    {
        private CurrentSource[] _currents;

        private void Start()
        {
            _currents = FindObjectsOfType<CurrentSource>();
        }

        public Vector3 SampleCurrentAtPosition(Vector3 position)
        {
            Vector3 result = default;
            foreach (var current in _currents)
            {
                result += current.SampleCurrentAtPosition(position);
            }

            return result;
        }
    }
}