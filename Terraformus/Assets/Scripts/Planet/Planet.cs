using System;
using UnityEngine;

/// <summary>
/// Class that manages the planet on a high level.
/// </summary>
public class Planet : MonoBehaviour
{

    ShapeBuilder ShapeBuilder;

    // How many triangles should be on each face  
    [Range(2, 256), Header("Planet Resolution")]
    public int resolution = 10;

    // Custom Settings Made from scriptable objects
    [Header("Custom Settings")]
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;

    [HideInInspector, SerializeField] MeshFilter[] meshFilters;
    PlanetFace[] planetFaces;

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

    void Initialize()
    {
        // Initialize the meshFilters, planetFaces, and ShapeBuilder if they are null.
        meshFilters ??= new MeshFilter[6];
        planetFaces ??= new PlanetFace[6];
        ShapeBuilder ??= new ShapeBuilder(shapeSettings, colorSettings);

        // Create the Meshes if they dont exist, and create the planet faces.
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
                CreateMeshFilter(i);

            // Create a new planet face if it does not exist.
            if (planetFaces[i] == null)
                planetFaces[i] = new PlanetFace(
                    ShapeBuilder, meshFilters[i].sharedMesh,
                    resolution, directions[i]
                );

            // Otherwise update the exisiting Face
            planetFaces[i].UpdatePlanetFace(
                meshFilters[i].sharedMesh,
                resolution, directions[i]
            );


        }
    }

    /// <summary>
    /// Used to create a MeshFilter 
    /// </summary>
    /// <param name="i"></param>
    private void CreateMeshFilter(int i)
    {
        // Create a new GameObject to store all the mesh data
        // Set its parent to the current object(Should be a planet face)
        GameObject meshObj = new GameObject("mesh " + i);
        meshObj.transform.parent = transform;

        // Adds a meshRenderer which is used to display the mesh
        // Assign it the standard ShaderMaterial
        meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

        // Give the object a MeshFilter which holds the mesh data, and assign it a new Mesh
        meshFilters[i] = meshObj.AddComponent<MeshFilter>();
        meshFilters[i].sharedMesh = new Mesh();
    }

    /// <summary>
    /// Called when the color settings change.
    /// Allows for dynamic updates from the editor
    /// </summary>
    public void OnColorChanged()
    {
        Initialize();
        GenerateColors();
    }

    /// <summary>
    /// Called when the shape settings change.
    /// Allows for dynamic updates from the editor
    /// </summary>
    public void OnShapeChanged()
    {
        Initialize();
        GenerateMesh();
    }

    /// <summary>
    /// Generates the planet based off of the shape and color settings.
    /// Should only be called if both color and shape setting are changed.
    /// Otherwise if only one of the setting types is changed call those methods.
    /// </summary>
    public void GeneratePlanet()
    {
        Initialize();
        GenerateMesh();
        GenerateColors();
    }

    /// <summary>
    /// Generates the mesh for each PlanetFace.
    /// calls the ConstructMesh() method from the PlanetFace class.
    /// </summary>
    void GenerateMesh()
    {
        foreach (PlanetFace face in planetFaces)
        {
            face.ConstructMesh();
        }
    }

    /// <summary>
    /// Sets each of the meshes to the mesh color in the color settings.
    /// TODO: Find a way to create some sort of color manager.
    /// The manager should set different colors based on the height of the mesh.
    /// </summary>
    void GenerateColors()
    {
        foreach (MeshFilter m in meshFilters)
        {
            m.GetComponent<MeshRenderer>().sharedMaterial.color = colorSettings.MeshColor;
        }
    }
}