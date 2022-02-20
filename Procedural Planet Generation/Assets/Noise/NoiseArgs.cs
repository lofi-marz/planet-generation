using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NoiseArgs
{
    public float strength = 1.0f;
    
    public float lacunarity = 2.0f;
    
    [Range(1,8)]
    public int octaves = 3;

    [Range(1,100)]
    public float period = 1;

    public float persistence = 0.5f;

    [Range(0,1)]
    public float minValue = 0.5f;

    public int seed;
}
