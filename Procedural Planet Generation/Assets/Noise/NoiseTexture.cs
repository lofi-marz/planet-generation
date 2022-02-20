using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTexture : MonoBehaviour
{

    private NoiseFilter noiseFilter = new NoiseFilter();
    // Start is called before the first frame update

    public NoiseTexture(NoiseArgs args)
    {
        noiseFilter = new NoiseFilter(args);
    }
    
}
