using System;
using System.Diagnostics;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] CubeMarcher cubeMarcher;
    [SerializeField] int chunkSize = 64;
    [SerializeField] int chunkCount = 1;
    [SerializeField] float scale = 1;

    [SerializeField] Transform player;
    CubeMarcher[][][] chunks;

    void CreateChunks()
    {
        chunks = new CubeMarcher[chunkCount][][];
        for (int x = 0; x < chunkCount; x++)
        {
            chunks[x] = new CubeMarcher[chunkCount][];
            for (int y = 0; y < chunkCount; y++)
            {
                chunks[x][y] = new CubeMarcher[chunkCount];
                for (int z = 0; z < chunkCount; z++)
                {
                    chunks[x][y][z] = Instantiate(cubeMarcher, new Vector3(x * (chunkSize-1), y * (chunkSize-1), z * (chunkSize-1))*scale, Quaternion.identity);
                    chunks[x][y][z].transform.parent = transform;
                }
            }
        }
    }
    void Start()
    {
        CreateChunks();
        for (int x = 0; x < chunkCount; x++)
        {
            for (int y = 0; y < chunkCount; y++)
            {
                for (int z = 0; z < chunkCount; z++)
                {
                    ComputeChunk(chunks[x][y][z]);
                }
            }
        }

    }
    void ComputeChunk(CubeMarcher chunk)
    {
        Vector3 position = chunk.transform.position;
        float[][][] voxels = new float[chunkSize][][];
        for (int x = 0; x < chunkSize; x++)
        {
            voxels[x] = new float[chunkSize][];
            for (int y = 0; y < chunkSize; y++)
            {
                voxels[x][y] = new float[chunkSize];
                for (int z = 0; z < chunkSize; z++)
                {
                    Vector3 worldPosition = position + new Vector3(x, y, z)*scale;
                    float worldValue = WorldValue(worldPosition);
                    voxels[x][y][z] = worldValue;
                }
            }
        }
        chunk.March(voxels,0.5f,scale);
    }
    float WorldValue(Vector3 position)
    {
        return Mathf.Clamp01(Mathf.PerlinNoise(position.x/10f, position.z/10f) - position.y/10f+1);
    }
}
