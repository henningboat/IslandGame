using System;
using UnityEngine;

namespace IslandGame.CurrentSystem
{
    public class CurrentSource : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
        
        
    }
}
