
 # Sphere Generation
Sphere mesh generation is annoying.
Essentially what we need to provide to Godot is a list of Vector3s, with every 3 being a triangle.
There's a few different ways to split a sphere into triangles


 ## UV Sphere
 - Divides a sphere into rectangles (which we then split into triangles) along the diagonal
 - We iterate around the points on a sphere using a given horizontal and vertical resolution
 - Calculate these using a spherical co-ordinate system
   - Uses a radius (here this is 1)
   - Two angles, which are basically the horizontal and vertical angles
 - So we're basically tracing around a sphere top to bottom, row by row

 - The benefit with this is the code is really simple so it was a good way for me to get to grips with 3D mesh generation

 - The downsides are it isn't the best way to generate a sphere. Points aren't evenly distributed, you get huge rectangles near the equator and a higher resolution closer to the poles.
 - Also my code is ugly

## Icosphere
 - Create an icosahedron
 - Split each face of the icosahedron into 4 (think triforce)
 - Normalize each vertex, to project it a sphere
 - Repeat

# Mesh Indexing
 - With an unindexed mesh, each vertex has a copy for each face it is a part of
 - This means that you get a uv and a normal for the vertex each face
 - So the faces don't blend together
 - With an indexed mesh, each face shares the copy of the vertex
 - The annoying part of this is actually indexing the vertices
 - My solution is:
   - Store a list of only the unique vertices
   - Store a dictionary which points each vertex to its index in the list (we could just search for the vertex in the list, but this is faster at the cost of a bit more space)
   - So then when you need a vertex, you use the cache to look up the index for it
   - The cache quietly adds the vertex to the cache if it hasn't been added already, and returns a new index
 