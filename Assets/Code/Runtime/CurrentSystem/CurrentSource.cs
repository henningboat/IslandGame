using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

namespace IslandGame.CurrentSystem
{
    public class CurrentSource : MonoBehaviour
    {
        [SerializeField] private float _strength;

        private void OnDrawGizmos()
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one * 2);
            Gizmos.DrawRay(Vector3.zero, Vector3.forward);
        }

        public Vector3 SampleCurrentAtPosition(float3 position)
        {
            var positionOS = transform.worldToLocalMatrix.MultiplyPoint(position);
            if (all(abs(positionOS) < 1))
            {
                return transform.forward * _strength;
            }

            return Vector3.zero;
        }
    }
}