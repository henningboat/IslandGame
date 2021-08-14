using Unity.Mathematics;
using UnityEngine;

namespace IslandGame.TerrainSystem
{
    public class SDFLine:SDFShape
    {
        [SerializeField] private float _radius = 5;

        private void OnDrawGizmosSelected()
        {
            Gizmos.matrix = transform.worldToLocalMatrix;
            Gizmos.DrawRay(Vector3.zero, Vector3.forward);
        }

        public override float Sample(float3 position)
        {
            float2 p = position.xz;
            float2 a = ((float3) (transform.position)).xz;
            float2 b = ((float3) (transform.position + transform.forward * transform.lossyScale.x)).xz;

            float2 pa = p - a, ba = b - a;
            float h = math.clamp(math.dot(pa, ba) / math.dot(ba, ba), 0.0f, 1.0f);
            return math.length(pa - ba * h)-_radius;

        }
    }
}