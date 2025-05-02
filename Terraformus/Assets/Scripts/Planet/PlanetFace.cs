using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;


public class PlanetFace
{
    FaceGenerator faceGenerator;
    Mesh mesh;

    // Used in the calculations of the vertices and triangles  
    int resolution;
    int triIndex;
    int[] triangles;


    // Vectors used to calculate the positioning of the face.
    // Normal Vector
    Vector3 faceNormal;
    // Orthogonal Vectors
    Vector3 A;
    Vector3 B;

    // used for shaders
    public List<float> faceElevations = new List<float>();


    public PlanetFace(FaceGenerator faceGenerator, Mesh mesh, int resolution, Vector3 faceNormal)
    {
        // Run some assertions to make sure the inputs are valid.
        Debug.Assert(faceGenerator != null, "PlanetFace: ShapeBuilder cannot be null");
        Debug.Assert(mesh != null, "PlanetFace: Mesh cannot be null");
        Debug.Assert(resolution > 1, "PlanetFace: Resolution must be greater than 1");
        Debug.Assert(faceNormal != null, "PlanetFace: FaceNormal cannot be null");

        this.mesh = mesh;
        this.resolution = resolution;
        this.faceNormal = faceNormal;
        this.faceGenerator = faceGenerator;

        A = new Vector3(faceNormal.y, faceNormal.z, faceNormal.x);
        B = Vector3.Cross(faceNormal, A);
    }

    /// <summary>
    /// Constructs the mesh for the terrain face.
    /// Uses the resolution to determine the number of vertices and triangles.
    /// </summary>
    public void ConstructMesh()
    {
        // The number of vertices = resolution * resolution
        Vector3[] vertices = new Vector3[resolution * resolution];
        Debug.Assert(
            vertices.Length == resolution * resolution,
            "PlanetFace: Vertices were not filled correctly"
        );

        // The number of triangles = number of squares on the face * 2 triangles per square * 3 vertices per triangle
        triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        Debug.Assert(
            triangles.Length == (resolution - 1) * (resolution - 1) * 6,
            "PlanetFace: Triangles were not filled correctly"
        );

        triIndex = 0;

        for (int y = 0, i = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++, i++)
            {
                // Original 'i' Calculations, Moved it to the for loop.
                //int i = x + y * resolution;

                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = faceNormal + 2 * ((percent.x - .5f) * A + (percent.y - .5f) * B);
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = faceGenerator.CalculatePosition(pointOnUnitSphere);

                // If we are not at the edge of the face, triangulate the face
                if (x != resolution - 1 && y != resolution - 1) { TriangulateFace(i); }
            }
        }

        Debug.Assert(triIndex == triangles.Length, "PlanetFace: Triangles were not filled correctly");
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        faceElevations = faceGenerator.elevationList
            .Skip(faceElevations.Count)
            .Take(resolution * resolution)
            .ToList();


        mesh.RecalculateNormals();
    }

    /// <summary>
    /// Splits up the squares of the planetFace into two triangles.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="System.NullReferenceException"></exception>
    private void TriangulateFace(int index)
    {
        Debug.Assert(triangles != null, "PlanetFace/TriangulateFace: triangles[] cannot be null");

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

