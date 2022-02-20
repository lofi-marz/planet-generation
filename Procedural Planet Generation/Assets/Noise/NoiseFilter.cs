using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    private Noise noise;
    private NoiseArgs args;

    public NoiseFilter()
    {
        noise = new Noise();
    }

    public NoiseFilter(NoiseArgs args)
    {
        noise = new Noise(args.seed);
        this.args = args;
        
      
    }
    public float Evaluate(Vector3 point)
    {
        float value = 0;
        float period = args.period;
        float strength = args.strength;
        for (int i = 0; i < args.octaves; i++)
        {

            value += ((noise.Evaluate(point * period) + 1f) / 2f) * strength;
            period *= args.lacunarity;
            strength *= args.persistence;
        }
        
        //I think the noise works out as a geometric sequence,
        var maxEstimate = ((1f - Math.Pow(args.persistence, 9)) / (1f - args.persistence)) * args.strength;
        if (value > ((1f - Math.Pow(args.persistence, 9)) / (1f - args.persistence)) * args.strength) throw new Exception($"{value} > {maxEstimate}");
        return Mathf.Max(value - args.minValue*args.strength, 0);

    }
}
