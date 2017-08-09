using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *      Generater random world
 *      - Can be chosen if the world has water or trees
 *      - Size is adjustable
 *      - Water level can be chosen
 *      - Seed can be used for saving the random world
 */
public class Generator : MonoBehaviour
{
    enum GroundTypes { GRASS, DIRT, WATER, AIR, TREE, EMPTY };

    public GameObject grassPrefab;
    public GameObject dirtPrefab;
    public GameObject waterPrefab;
    public GameObject[] treePrefabs;

    // width and depth of the terrain
    // bigger range will have fewer hight hills
    public int maxX = 10,
               maxZ = 10,
               range = 15;

    public int seed = 0;

    public bool worldHasWater = true;
    public int waterLevel = 3;

    public bool worldHasTrees = true;

    private GroundTypes[,,] worldObjs;
    private List<AvailableGround> availablGround;
    private int tempY;

    // Saves all ground positions where trees can be spawned
    class AvailableGround
    {
        public AvailableGround(int x, int z, int y)
        {
            xPos = x;
            zPos = z;
            yPos = y;
        }
        public int xPos { get; set; }
        public int zPos { get; set; }
        public int yPos { get; set; }
    }

    private void Start()
    {
        availablGround = new List<AvailableGround>();
        InitialiseArray();

        // If seed is 0, generate new seed
        // otherwise use the same seed as previously
        if (seed == 0)
            seed = Random.Range(0, 1000000);

        GenerateMap();
        InstantiateObjs();
        if(worldHasTrees)
            GenerateTrees();
    }

    void InitialiseArray()
    {
        // Gives some space for noise wave
        tempY = 15;

        worldObjs = new GroundTypes[maxX, tempY, maxZ];

        for (int i = 0; i < maxX; i++)
        {
            for (int k = 0; k < maxZ; k++)
            {
                for (int j = 0; j < tempY; j++)
                {
                    worldObjs[i, j, k] = GroundTypes.AIR;
                }
            }
        }
    }

    // Saves all the blocks, their types and positions
    void GenerateMap()
    {
        // Generates gameobjects to x, y and z positions
        for (int x = 0; x < maxX; x++)
        {
            for (int z = 0; z < maxZ; z++)
            {
                // +2 so there's always some land (or water) at the bottom
                int columnHeight = GenerateNoise(x, z) + 2;

                for (int y = 0; y < columnHeight; y++)
                {
                    // Makes sure the whole lake is filled with water
                    if (worldHasWater && y <= waterLevel)
                    {
                        y = waterLevel;
                        worldObjs[x, y, z] = GroundTypes.WATER;
                    }
                    else if (y == columnHeight - 1)
                    {
                        availablGround.Add(new AvailableGround(x,z,y));
                        worldObjs[x, y, z] = GroundTypes.GRASS;
                    }
                    else if (y >= columnHeight)
                        worldObjs[x, y, z] = GroundTypes.AIR;
                    else
                        worldObjs[x, y, z] = GroundTypes.DIRT;
                }
            }
        }
    }

    // Generates perlin noise which is used for randomization
    public int GenerateNoise(int x, int z)
    {
        float noiseSize = range;
        float noiseVal = Mathf.PerlinNoise(seed + x / noiseSize, seed + z / noiseSize);
        noiseVal *= 10;
        return (int)noiseVal;
    }

    // Instantiates the gameobjects
    void InstantiateObjs()
    {
        bool visible = false;

        for (int x = 0; x < maxX; x++)
        {
            for (int z = 0; z < maxZ; z++)
            {
                for (int y = 0; y < tempY; y++)
                {
                    // Checks if adjacent blocks are AIR
                    if (y < tempY-1 && x < maxX - 1 && z < maxZ - 1 && y > 0 && x > 0 && z > 0)
                    {
                        if (worldObjs[x, y + 1, z] == GroundTypes.AIR || worldObjs[x + 1, y, z] == GroundTypes.AIR || worldObjs[x, y, z + 1] == GroundTypes.AIR)
                        {
                            visible = true;
                        }
                        else if (worldObjs[x, y - 1, z] == GroundTypes.AIR || worldObjs[x - 1, y, z] == GroundTypes.AIR || worldObjs[x, y, z - 1] == GroundTypes.AIR)
                        {
                            visible = true;
                        }
                    }

                    if (x == 0 || x == maxX - 1 || z == 0 || z == maxZ - 1 || visible == true)
                    {

                        switch (worldObjs[x, y, z])
                        {
                            case GroundTypes.AIR:
                                break;
                            case GroundTypes.DIRT:
                                GameObject dirt = Instantiate(dirtPrefab, new Vector3(x, y, z), Quaternion.identity);
                                dirt.transform.parent = gameObject.transform;
                                break;
                            case GroundTypes.GRASS:
                                GameObject grass = Instantiate(grassPrefab, new Vector3(x, y, z), Quaternion.identity);
                                grass.transform.parent = gameObject.transform;
                                break;
                            case GroundTypes.WATER:
                                GameObject water = Instantiate(waterPrefab, new Vector3(x, y, z), Quaternion.identity);
                                water.transform.parent = gameObject.transform;
                                break;
                            default:
                                Debug.Log("This shouldn't be the case..");
                                break;
                        }
                        visible = false;
                    }
                }
            }
        }
    }

    // Generates random tree at random groundpoint
    void GenerateTrees()
    {
        Random.InitState(seed);
        int randAmountOfTrees = Random.Range(3, maxX);

        for (int i = 0; i < randAmountOfTrees; i++)
        {
            int randPosOfTree = Random.Range(0, availablGround.Count);
            int randTree = Random.Range(0, treePrefabs.Length);
            int randRotation = Random.Range(0, 360);

            Vector3 treePos = new Vector3(availablGround[randPosOfTree].xPos, availablGround[randPosOfTree].yPos + 1, availablGround[randPosOfTree].zPos);

            GameObject tree = Instantiate(treePrefabs[randTree], treePos, Quaternion.Euler(0f, randRotation, 0f));
            tree.transform.parent = gameObject.transform;
            
            availablGround.RemoveAt(randPosOfTree);
            worldObjs[availablGround[randPosOfTree].xPos, availablGround[randPosOfTree].yPos, availablGround[randPosOfTree].zPos] = GroundTypes.TREE;
        }
    }
}

