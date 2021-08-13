using Unity.Mathematics;

namespace IslandGame.TerrainSystem
{
    public class SDFCircle:SDFShape
    {
        public override float Sample(float3 position)
        {
            return math.length(position - (float3) transform.position) - transform.lossyScale.x;
        }
    }
}