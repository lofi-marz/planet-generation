using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class Planet : MonoBehaviour
{

    /**
     * The idea with this is you put in an index, get out a vertex
     * If you put in a vertex, you get back an index, which is either created or a lookup
     */
    class VertexCache
    {
        private int index;
        
        public Dictionary<Vector3, int> indexes;
        public List<Vector3> UniqueVertices;
        public int GetIndex(Vector3 v)
        {
            AddVertex(v);
            
            return indexes[v];
        }

        public void AddVertex(Vector3 v)
        {
            
            if (!indexes.ContainsKey(v))
            {
                indexes.Add(v, index);
                UniqueVertices.Add(v);
                if (index != UniqueVertices.Count-1) throw new Exception();
                index++;
            }
        }

        public Vector3 GetVertex(int i)
        {
            return UniqueVertices[i];
        }

        public VertexCache()
        {
            index = 0;
            indexes = new Dictionary<Vector3, int>();
            UniqueVertices = new List<Vector3>();
        }
    }

    private VertexCache cache;
    
    List<Vector3> newNormals = new List<Vector3>();
    List<Vector2> newUV = new List<Vector2>();
    List<int> newTriangles = new List<int>();
    
    [SerializeField]
    private NoiseArgs noiseArgs = new NoiseArgs();
    
    [Range(0, 8)]
    public int subdivisions = 1;

    private NoiseFilter noiseFilter;

    private void OnValidate()
    {
        noiseFilter = new NoiseFilter(noiseArgs);
        GenerateMesh();
    }
    
    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = "Icosahedron";
        GetComponent<MeshFilter>().mesh = mesh;
        cache = new VertexCache();
        newNormals = new List<Vector3>();
        newUV = new List<Vector2>(); 
        newTriangles = new List<int>();
        GenerateIcosahedron();


        for (int i = 0; i < subdivisions; i++)
        {
            SplitTriangles();
        }
        
        for (var i = 0; i < cache.UniqueVertices.Count; i++)
        {
            var v = cache.UniqueVertices[i];
            var noise = noiseFilter.Evaluate(v);
            cache.UniqueVertices[i] *= 1+noise;
        }
        
        //IndexMesh();

        mesh.vertices = cache.UniqueVertices.ToArray();
        mesh.normals = newNormals.ToArray();
        mesh.uv = newUV.ToArray();
        mesh.triangles = newTriangles.ToArray();
        mesh.RecalculateNormals();
    }

    private void GenerateIcosahedron()
    {
        
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
        foreach (var v in vertices)
        {
            cache.AddVertex(v);
        }
        foreach (var face in faces)
        {
            newTriangles.Add(face[0]);
            newTriangles.Add(face[1]);
            newTriangles.Add(face[2]);
        }

    }
    private void SplitTriangles()
    {
        
        
        //Debug.Log(oldVertices.Count);
        var splitTriangles = new List<int>();
   
        for (var i = 0; i < newTriangles.Count - 2; i+=3)
        {
            var point1 = cache.GetVertex(newTriangles[i]);
            var point2 = cache.GetVertex(newTriangles[i + 1]);
            var point3 = cache.GetVertex(newTriangles[i + 2]);

            var mid1 = ((point1 + point2) / 2);
            var mid2 = ((point2 + point3) / 2);
            var mid3 = ((point3 + point1) / 2);

            var newPoints = new Vector3[]
            {
                point1, mid1, mid3,
                mid1, point2, mid2,
                mid3, mid2, point3,
                mid1, mid2, mid3
            };
            
            for (var j = 0; j < newPoints.Length; j++)
            {
      
                splitTriangles.Add(cache.GetIndex(newPoints[j]));
            }
        }

        newTriangles = splitTriangles;
        
    }
    
    private static Vector3 SphericalToCartesian(float theta1, float theta2)
    {
        // Here we're converting our polar and azimuthal angle to x y z co-ordinates
        // since we're dealing with a normalised shape here, we don't need the magnitude
        var x = (float)(Math.Sin(theta1) * Math.Cos(theta2));
        var y = (float)Math.Cos(theta1);
        var z = (float)(Math.Sin(theta1) * Math.Sin(theta2));
        return new Vector3(x, y, z);
    }
    
}
