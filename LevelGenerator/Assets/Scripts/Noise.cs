using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noise {

    long seed;

    public Noise(long seed)
    {
        this.seed = seed;
    }

    public int GetNoise(int x, int z, int amplitude)
    {
        //  int frequency = 16;

        // float noise = 0;
        //  amplitude /= 2;

        //  while (frequency > 0)
        //{
        /*  int frequencyIndex = x / frequency;

          float progress = (x % frequency) / (frequency * 1f);

          float leftRandom = RandomNum(frequencyIndex, amplitude);
          float rightRandom = RandomNum(frequencyIndex + 1, amplitude);

          noise += (1 - progress) * leftRandom + progress * rightRandom;

          frequency /= 2;
          amplitude /= 2;

          amplitude = Mathf.Max(1, amplitude);*/
        //  }
        //  return (int)Mathf.Round(noise);
        /*
         float noise2 = Mathf.PerlinNoise(x, z);

         noise2 = noise2* amplitude;
         Debug.Log("noise = " + noise);
         Debug.Log("noise2 = " + noise2 + "x = " + x + "z = " + z);
         return (int)Mathf.Max(noise2);*/

        float noiseSize = amplitude;
        float noiseVal = Mathf.PerlinNoise(seed + x/noiseSize, seed + z/noiseSize);
        Debug.Log("noiseval = " + noiseVal + ", x = " + x + ", z = " + z);
        noiseVal *= 10;
        Debug.Log("uusi noiseval = " + noiseVal + ", x = " + x + ", z = " + z);
        return (int)noiseVal;
        
    }

    private int RandomNum(int index, int range)
    {
        return (int)((index + seed) ^ 5) % range;
    }
}
