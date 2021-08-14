using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IslandGame.Utils;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace IslandGame.TerrainSystem
{
    [ExecuteInEditMode]
    public class TerrainGenerator : Singleton<TerrainGenerator>
    {
        [SerializeField] private AnimationCurve _distanceMapping;

        private static readonly int2 TerrainSize = new int2(50, 50);
        private Mesh _mesh;
        private float _samplePosition;

       [SerializeField] private List<MeshFilter> _terrainMeshes;


        struct VertexData
        {
            public float3 position;
            public float3 normal;
            public float vertexColor;
        }

        private void Update()
        {
            if (Application.isPlaying && Time.frameCount > 2)
            {
                return;
            }
            if (_mesh == null)
            {
                _mesh = new Mesh {name = "ProceduralTerrain"};
                _mesh.hideFlags = HideFlags.DontSave;
            }
            
            var meshWriter = Mesh.AllocateWritableMeshData(1);
            var data = meshWriter[0];
            var sdfShapes = FindObjectsOfType<SDFShape>();
        
            //get heightmap
            NativeArray<float> heightmap = new NativeArray<float>( TerrainSize.x * TerrainSize.y,Allocator.Temp);
            for (int x = 0; x < TerrainSize.x; x++)
            {
                for (int z = 0; z < TerrainSize.y; z++)
                {
                    float height = SampleAtPosition(x,z, sdfShapes);
                    WriteHeightmap(heightmap, x, z, height);
                }
            }

            int quadCount = TerrainSize.x * TerrainSize.y;
            int vertexCount = quadCount * 4;
            int indexCount = quadCount * 6;
            data.SetVertexBufferParams(vertexCount,
                new[]
                {
                    new VertexAttributeDescriptor(VertexAttribute.Position), 
                    new VertexAttributeDescriptor(VertexAttribute.Normal),
                    new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 1)
                });
            data.SetIndexBufferParams(indexCount, IndexFormat.UInt32);

            var vertexData = data.GetVertexData<VertexData>();
            var indexData = data.GetIndexData<uint>();
            
            //build mesh
            for (int x = 0; x < TerrainSize.x; x++)
            {
                for (int z = 0; z < TerrainSize.y; z++)
                {
                    int vertexIndex = (x * TerrainSize.x + z) * 4;

                    vertexData[vertexIndex + 0] = SampleHeightmapToVertexData(heightmap,x,z);
                    vertexData[vertexIndex + 1] = SampleHeightmapToVertexData(heightmap,x+1,z);
                    vertexData[vertexIndex + 2] = SampleHeightmapToVertexData(heightmap,x,z+1);
                    vertexData[vertexIndex + 3] = SampleHeightmapToVertexData(heightmap,x+1,z+1);
                    
                    int indexIndex = (x * TerrainSize.x + z) * 6;

                    indexData[indexIndex + 0] =(uint) vertexIndex + 2;
                    indexData[indexIndex + 1] =(uint) vertexIndex + 1;
                    indexData[indexIndex + 2] =(uint) vertexIndex + 0;
                    indexData[indexIndex + 3] =(uint) vertexIndex + 1;
                    indexData[indexIndex + 4] =(uint) vertexIndex + 2;
                    indexData[indexIndex + 5] =(uint) vertexIndex + 3;
                }
            }
            
            data.subMeshCount = 1;
            data.SetSubMesh(0,new SubMeshDescriptor(0,indexCount));

            Mesh.ApplyAndDisposeWritableMeshData(meshWriter, _mesh);
            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            
            heightmap.Dispose();

            GetComponent<MeshCollider>().sharedMesh = _mesh;

            foreach (var terrainMeshes in _terrainMeshes)
            {
                terrainMeshes.sharedMesh = _mesh;
            }
        }

        private VertexData SampleHeightmapToVertexData(NativeArray<float> heightmap, int x, int z)
        {
            VertexData vertexData = default;
            float distance = SampleHeightmap(heightmap, x, z);
            vertexData.vertexColor = distance;

            float height = _distanceMapping.Evaluate(-distance);

            vertexData.position = new float3(x, height, z);
            return vertexData;
        }

        private float SampleHeightmap(NativeArray<float> heightmap, int x, int z)
        {
            x = math.clamp(x,0, TerrainSize.x - 1);
            z = math.clamp(z,0, TerrainSize.y - 1);
            return heightmap[x * TerrainSize.x + z];
        }

        private void WriteHeightmap(NativeArray<float> heightmap, int x, int z, float height)
        {
            heightmap[x * TerrainSize.x + z] = height;
        }

        private float SampleAtPosition(int x, int y, SDFShape[] sdfShapes)
        {
            var position = new float3(x, 0, y);
            float distance = sdfShapes.Select(shape => { return shape.Sample(position); }).Min();

            distance += noise.snoise(position * 0.2f);

            return distance;
        }
    }
}
