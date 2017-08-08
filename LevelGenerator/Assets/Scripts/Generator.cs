using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject grassPrefab;
    public GameObject dirtPrefab;

    public int maxX = 10,
               maxZ = 10,
               maxY = 10;

    public int seed = 0;

    Noise noise;

    private void Awake()
    {

    }

    private void Start()
    {
        seed = Random.Range(0, 1000000);
        noise = new Noise(seed);
        GenerateMap();
    }

    void GenerateMap()
    {
        // Generates gameobjects to x, y and z positions
        for (int x = 0; x < maxX; x++)
        {
            for (int z = 0; z < maxZ; z++)
            {
                int columnHeight = noise.GetNoise(x, z, maxY) + 2;
                for (int y = 0; y < columnHeight; y++)
                {
                        GameObject gameObjToInstantiate = dirtPrefab;
                        if (y == columnHeight - 1)
                            gameObjToInstantiate = grassPrefab;
                        GameObject obj = Instantiate(gameObjToInstantiate, new Vector3(x, y, z), Quaternion.identity);
                        obj.transform.parent = this.gameObject.transform;
                    
                }
                
            }
        }
    }
}
