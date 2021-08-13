using Unity.Mathematics;
using UnityEngine;

namespace IslandGame.TerrainSystem
{
    public abstract class SDFShape : MonoBehaviour
    {
        public abstract float Sample(float3 position);
    }
}
