using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Texture2D))]
public class NoiseEditor : Editor
{
    private NoiseFilter noiseFilter;
    private Texture2D t;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    private void OnEnable()
    {
        noiseFilter = new NoiseFilter();
        t = (Texture2D) (this.target);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
