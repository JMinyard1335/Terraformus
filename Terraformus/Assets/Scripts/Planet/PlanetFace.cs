using UnityEngine;

public class PlanetFace
{

    ShapeBuilder shapeBuilder;
    Mesh mesh;

    int resolution;
    int[] triangles;
    int triIndex;


    Vector3 faceNormal; // This is the normal vector to the face.
    // These are the vectors orthogonal to the face normal. togethers these define the plane of the face.
    Vector3 A;
    Vector3 B;


    public PlanetFace(ShapeBuilder ss, Mesh m, int resolution, Vector3 faceNormal)
    {
        mesh = m;
        this.resolution = resolution;
        this.faceNormal = faceNormal;
        shapeBuilder = ss;

        A = new Vector3(faceNormal.y, faceNormal.z, faceNormal.x);
        B = Vector3.Cross(faceNormal, A);
    }

    public void UpdatePlanetFace(Mesh m, int resolution, Vector3 faceNormal)
    {
        mesh = m;
        this.resolution = resolution;
        this.faceNormal = faceNormal;

        A = new Vector3(faceNormal.y, faceNormal.z, faceNormal.x);
        B = Vector3.Cross(faceNormal, A);
    }

    /// <summary>
    /// Constructs the mesh for the terrain face.
    /// Uses the resolution to determine the number of vertices and triangles.
    /// </summary>
    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        triangles ??= new int[(resolution - 1) * (resolution - 1) * 6];
        triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = faceNormal + (percent.x - .5f) * 2 * A + (percent.y - .5f) * 2 * B;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = shapeBuilder.CalculatePosition(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    TriangulateFace(i);
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    /// <summary>
    /// Splits up the squares of the planetFace into two triangles.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="System.NullReferenceException"></exception>
    private void TriangulateFace(int index)
    {
        if (triangles == null)
            throw new System.NullReferenceException("Triangles array is null.");


        // Create the triangles
        triangles[triIndex] = index;
        triangles[triIndex + 1] = index + resolution + 1;
        triangles[triIndex + 2] = index + resolution;

        triangles[triIndex + 3] = index;
        triangles[triIndex + 4] = index + 1;
        triangles[triIndex + 5] = index + resolution + 1;

        // Move the index to the next set of triangles
        triIndex += 6;
    }
}

