using UnityEngine;

public class TerrainFace
{

    ShapeSpawner shapeSpawner;
    Mesh mesh;
    int resolution;

    Vector3 faceNormal; // This is the normal vector to the face.
    // These are the vectors perpendicular to the face normal That make up the space.
    Vector3 A;
    Vector3 B;

    public TerrainFace(ShapeSpawner ss, Mesh m, int resolution, Vector3 faceNormal)
    {
        mesh = m;
        this.resolution = resolution;
        this.faceNormal = faceNormal;
        shapeSpawner = ss;

        A = new Vector3(faceNormal.y, faceNormal.z, faceNormal.x);
        B = Vector3.Cross(faceNormal, A);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int i = x + y * resolution;
                Vector2 percent = new Vector2(x, y) / (resolution - 1);
                Vector3 pointOnUnitCube = faceNormal + (percent.x - .5f) * 2 * A + (percent.y - .5f) * 2 * B;
                Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                vertices[i] = shapeSpawner.CalculatePosition(pointOnUnitSphere);

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = i;
                    triangles[triIndex + 1] = i + resolution + 1;
                    triangles[triIndex + 2] = i + resolution;

                    triangles[triIndex + 3] = i;
                    triangles[triIndex + 4] = i + 1;
                    triangles[triIndex + 5] = i + resolution + 1;
                    triIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}