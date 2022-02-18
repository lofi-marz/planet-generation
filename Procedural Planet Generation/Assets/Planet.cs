using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{

    class VertexCache
    {
        private int index;
        public Dictionary<Vector3, int> indexes;
        public List<Vector3> Vertices;
        public int GetIndex(Vector3 v)
        {
            AddVertex(v);
            return indexes[v];
        }

        public void AddVertex(Vector3 v)
        {
            if (!indexes.ContainsKey(v))
            {
                indexes.Add(v, index++);
                Vertices.Add(v);
            }
        }

        public Vector3 GetVertex(int i)
        {
            return Vertices[i];
        }

        public VertexCache()
        {
            index = 0;
            indexes = new Dictionary<Vector3, int>();
            Vertices = new List<Vector3>();
        }
    }

    List<Vector3> newVertices = new List<Vector3>();
    List<Vector3> newNormals = new List<Vector3>();
    List<Vector2> newUV = new List<Vector2>();
    List<int> newTriangles = new List<int>();

    [Range(0, 5)]
    public int Subdivisions = 1;


    private void OnValidate()
    {
        GenerateMesh();
    }

    void IndexMesh()
    {
        //Put in the old index to see if it maps to a new index
        var indexMap = new Dictionary<int, int>();
        var uniqueVertices = new List<Vector3>();
        
        var oldLength = newVertices.Count;
        for (var i = 0; i < newVertices.Count; i++)
        {
            if (uniqueVertices.Contains(newVertices[i]))
            {
                indexMap[i] = uniqueVertices.IndexOf(newVertices[i]);
            }
            else
            {
                uniqueVertices.Add(newVertices[i]);
            }
        }

        
        for (var i = 0; i < newTriangles.Count; i++)
        {
            //Get the old index from new triangles
            //If we have a mapping for it, then replace it with the new value
            //If we don't it should have been unique in the first place
            var oldIndex = newTriangles[i];
            var newIndex = uniqueVertices.IndexOf(newVertices[oldIndex]);
            newTriangles[i] = newIndex;
      
        }
        newVertices = uniqueVertices;
        

        
    }
    
    // Start is called before the first frame update
    void Start()
    {

       
    }

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Icosahedron";
        GetComponent<MeshFilter>().mesh = mesh;
        
        GenerateIcosahedron();


        for (int i = 0; i < Subdivisions; i++)
        {
            SplitTriangles();
        }
        
        IndexMesh();
        
        mesh.vertices = newVertices.ToArray();
        mesh.normals = newNormals.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void GenerateIcosahedron()
    {
        newVertices = new List<Vector3>();
        float phi = (1f + Mathf.Sqrt(5f)) / 2f;
        //Generate the vertices
        var vertices = new List<Vector3>();
        vertices.Add(new Vector3(-1,  phi,  0));
        vertices.Add(new Vector3( 1,  phi,  0));
        vertices.Add(new Vector3(-1, -phi,  0));
        vertices.Add(new Vector3( 1, -phi,  0));

        vertices.Add(new Vector3( 0, -1,  phi));
        vertices.Add(new Vector3( 0,  1,  phi));
        vertices.Add(new Vector3( 0, -1, -phi));
        vertices.Add(new Vector3( 0,  1, -phi));

        vertices.Add(new Vector3( phi,  0, -1));
        vertices.Add(new Vector3( phi,  0,  1));
        vertices.Add(new Vector3(-phi,  0, -1));
        vertices.Add(new Vector3(-phi,  0,  1));
        vertices.Add(new Vector3(0, 1, 0));
        vertices.Add(new Vector3(1, 0, 0));
        vertices.Add(new Vector3(0, 0, 1));
        
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
            newVertices.Add(vertices[face[0]]);
            newVertices.Add(vertices[face[1]]);
            newVertices.Add(vertices[face[2]]);
        }
        for (var i = 0; i < newVertices.Count; i++)
        {
            newTriangles.Add(i);
        }
    }
    private void SplitTriangles()
    {

        var oldVertices = new List<Vector3>(newVertices);
        newVertices.Clear();
        Debug.Log(oldVertices.Count);
        newTriangles = new List<int>();
        newUV = new List<Vector2>();
        
        for (var i = 0; i < oldVertices.Count - 2; i+=3)
        {
            var point1 = oldVertices[i];
            var point2 = oldVertices[i + 1];
            var point3 = oldVertices[i + 2];
            
            var mid1 = ((point1 + point2)/2);
            var mid2 = (point2 + point3)/2;
            var mid3 = (point3 + point1)/2;
            
            newVertices.AddRange(new Vector3[] {point1, mid1, mid3});
            newVertices.AddRange(new Vector3[] {mid1, point2, mid2});
            newVertices.AddRange(new Vector3[] {mid3, mid2, point3});
            newVertices.AddRange(new Vector3[] {mid1, mid2, mid3});
        }
        
       
        for (var i = 0; i < newVertices.Count; i++)
        {
            newVertices[i] = newVertices[i] / newVertices[i].magnitude;
            newTriangles.Add(i);
        }
        




    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
