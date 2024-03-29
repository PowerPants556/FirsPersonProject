using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLvlGenerator : MonoBehaviour
{
    [SerializeField] private GameObject groundPref, grassPref;
    private int baseHeight = 2, maxBlocksCountY = 10, chunkSize = 16, perlinNoiseSensetivity = 25, chunkCount = 4;
    private float seedX, seedY;
    private void Start()
    {
        seedY = Random.Range(0, 10);
        seedX = Random.Range(0, 10);
        for (int x = 0; x < chunkCount; x++)
        {
            for (int z = 0; z < chunkCount; z++)
            {
                CreateChunk(x, z);
            }
        }
    }

    private void Update()
    {
        
    }
    private void CreateChunk(int chunkNumX, int chunkNumZ)
    {
        GameObject chunk = new GameObject();
        float chunkX = chunkNumX * chunkSize + chunkSize / 2;
        float chunkZ = chunkNumZ * chunkSize + chunkSize / 2;
        chunk.transform.position = new Vector3(chunkX, 0, chunkZ);
        chunk.name = $"Chunk: {chunkX}, {chunkZ}";
        chunk.AddComponent<Chunk>();
        chunk.AddComponent<MeshFilter>();
        chunk.AddComponent<MeshRenderer>();

        for (int x = chunkNumX * chunkSize; x < chunkNumX * chunkSize + chunkSize; x++)
        {
            for (int z = chunkNumZ * chunkSize; z < chunkNumZ * chunkSize + chunkSize; z++)
            {
                float xSample = seedX + (float)x / perlinNoiseSensetivity;
                float ySample = seedY + (float)z / perlinNoiseSensetivity;
                float sample = Mathf.PerlinNoise(xSample, ySample);
                int height = baseHeight + (int)(sample * maxBlocksCountY);

                for (int y = 0; y < height; y++)
                {
                    GameObject obj = null;
                    if(y == height - 1)
                    {
                        obj = Instantiate(grassPref, new Vector3(x, y, z), Quaternion.identity);
                    }
                    else
                    {
                        obj = Instantiate(groundPref, new Vector3(x, y, z), Quaternion.identity);
                    }
                    obj.transform.SetParent(chunk.transform);
                }
            }
        }
    }
}
