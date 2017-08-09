using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{

    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject waterPrefab;

    // width and depth of the terrain
    // bigger range will have fewer hight hills
    public int maxX = 10,
               maxZ = 10,
               range = 15;

    public int seed = 0;

    public bool worldHasWater = true;

    Noise noise;

    private void Start()
    {
        // If seed is 0, generate new seed
        // otherwise use the same seed as previously
        if (seed == 0)
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
                // +2 so there's always some land (or water) at the bottom
                int columnHeight = noise.GetNoise(x, z, range) + 2;

                for (int y = 0; y < columnHeight; y++)
                {
                    InstantiateObj(x, y, z, columnHeight);
                }
            }
        }
    }

    // Instantiates the gameobjects
    void InstantiateObj(int x, int y, int z, int columnHeight)
    {
        int newY = y;
        // Instantiate only to visible points (leave the map hollow)
        if (x == 0 || x == maxX - 1 || z == 0 || z == maxZ - 1 || y == columnHeight - 1)
        {

            GameObject gameObjToInstantiate = dirtPrefab;
            if (y == columnHeight - 1)
                gameObjToInstantiate = grassPrefab;
            if (worldHasWater && y <= 3)
            {
                newY = 3;
                gameObjToInstantiate = waterPrefab;
            }
            GameObject obj = Instantiate(gameObjToInstantiate, new Vector3(x, newY, z), Quaternion.identity);
            obj.transform.parent = this.gameObject.transform;
        }
    }
}
