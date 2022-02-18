using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ruby : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        var t = GetComponent<Transform>();
        t.Rotate(0, 100f, 0);
    }
}
