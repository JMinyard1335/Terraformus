using System;
using UnityEngine;

/// <summary>
/// Class that manages the planet on a high level.
/// Things like manageing each of the faces and their responabilities.
/// </summary>
public class CubePlanet : MonoBehaviour {

    ShapeSpawner shapeSpawner;

    // How many triangles should be on each face  
    [Range(2,256), Header("Planet Resolution")]
    public int resolution = 10;

    // Custom Settings Made from scriptable objects
    [Header("Custom Settings")]
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    // Used to allow the inspector menus to collapse
    [HideInInspector] public bool colorFoldout = true;
    [HideInInspector] public bool shapeFoldout = true;

    Vector3[] directions = { 
        Vector3.up, 
        Vector3.down, 
        Vector3.left, 
        Vector3.right, 
        Vector3.forward, 
        Vector3.back 
    };
     
	private void OnValidate() {
        GeneratePlanet();
	}

	void Initialize() {
        // Only create the MeshFilters if they don't already exist
        if (meshFilters == null || meshFilters.Length == 0)
            meshFilters = new MeshFilter[6];

        terrainFaces = new TerrainFace[6];
        shapeSpawner = new ShapeSpawner(shapeSettings); 

        // Create the MeshFilters and Meshes for each of the 6 faces
        for (int i = 0; i < 6; i++) {
            CreateMeshFilter(i);

            terrainFaces[i] = new TerrainFace(shapeSpawner, meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    /// <summary>
    /// Used to create a meshFilter if it doesn't already exist.
    /// </summary>
    /// <param name="i"></param>
    private void CreateMeshFilter(int i) {
        // If the meshfilter already exists, return
        if (meshFilters[i] != null) return;

        GameObject meshObj = new GameObject("mesh " + i);
        meshObj.transform.parent = transform;
        meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

        meshFilters[i] = meshObj.AddComponent<MeshFilter>();
        meshFilters[i].sharedMesh = new Mesh();
    }

    // Call when the color settings change
    public void OnColorChanged() {   
        Initialize();
        GenerateColors();
    }

    // Call when the shape settings change
    public void OnShapeChanged() {
        Initialize();
        GenerateMesh();
    } 

    // Call when the planet is generated
    public void GeneratePlanet() {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    void GenerateMesh() {
        foreach (TerrainFace face in terrainFaces) {
            face.ConstructMesh();
        }
    }

    void GenerateColors() {
        foreach (MeshFilter m in meshFilters) {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.MeshColor;
        }
    }
}