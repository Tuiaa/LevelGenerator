using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {

    long seed;

    public Noise(long seed)
    {
        this.seed = seed;
    }

    public int GetNoise(int x, int z, int range)
    {
        float noiseSize = range;
        float noiseVal = Mathf.PerlinNoise(seed + x/noiseSize, seed + z/noiseSize);
        Debug.Log("noiseval = " + noiseVal + ", x = " + x + ", z = " + z);
        noiseVal *= 10;
        Debug.Log("uusi noiseval = " + noiseVal + ", x = " + x + ", z = " + z);
        return (int)noiseVal;
        
    }
}
