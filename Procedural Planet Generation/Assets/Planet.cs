using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{
    List<Vector3> newVertices = new List<Vector3>();
    List<Vector3> newNormals = new List<Vector3>();
    List<Vector2> newUV = new List<Vector2>();
    List<int> newTriangles = new List<int>();
    // Start is called before the first frame update
    void Start()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Icosahedron";
        GetComponent<MeshFilter>().mesh = mesh;
        
        float phi = (1f + Mathf.Sqrt(5f)) / 2f;
        //Generate the vertices

        newVertices.Add(new Vector3(-1,  phi,  0));
        newVertices.Add(new Vector3( 1,  phi,  0));
        newVertices.Add(new Vector3(-1, -phi,  0));
        newVertices.Add(new Vector3( 1, -phi,  0));

        newVertices.Add(new Vector3( 0, -1,  phi));
        newVertices.Add(new Vector3( 0,  1,  phi));
        newVertices.Add(new Vector3( 0, -1, -phi));
        newVertices.Add(new Vector3( 0,  1, -phi));

        newVertices.Add(new Vector3( phi,  0, -1));
        newVertices.Add(new Vector3( phi,  0,  1));
        newVertices.Add(new Vector3(-phi,  0, -1));
        newVertices.Add(new Vector3(-phi,  0,  1));
        newVertices.Add(new Vector3(0, 1, 0));
        newVertices.Add(new Vector3(1, 0, 0));
        newVertices.Add(new Vector3(0, 0, 1));
        
        // create 20 triangles of the icosahedron
        var faces = new List<int[]>();

        // 5 faces around point 0
        faces.Add(new int[]{0, 11, 5});
        faces.Add(new int[]{0, 5, 1});
        faces.Add(new int[]{0, 1, 7});
        faces.Add(new int[]{0, 7, 10});
        faces.Add(new int[]{0, 10, 11});

        // 5 adjacent faces
        faces.Add(new int[]{1, 5, 9});
        faces.Add(new int[]{5, 11, 4});
        faces.Add(new int[]{11, 10, 2});
        faces.Add(new int[]{10, 7, 6});
        faces.Add(new int[]{7, 1, 8});

        // 5 faces around point 3
        faces.Add(new int[]{3, 9, 4});
        faces.Add(new int[]{3, 4, 2});
        faces.Add(new int[]{3, 2, 6});
        faces.Add(new int[]{3, 6, 8});
        faces.Add(new int[]{3, 8, 9});

        // 5 adjacent faces
        faces.Add(new int[]{4, 9, 5});
        faces.Add(new int[]{2, 4, 11});
        faces.Add(new int[]{6, 2, 10});
        faces.Add(new int[]{8, 6, 7});
        faces.Add(new int[]{9, 8, 1});
        
        foreach (var face in faces)
        {
            newTriangles.AddRange(face);
        }

        /*for (int i = 0; i < newVertices.Count; i++)
        {
            newNormals.Add(newVertices[i]);
        }*/
        
        
        mesh.vertices = newVertices.ToArray();
        mesh.normals = newNormals.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
