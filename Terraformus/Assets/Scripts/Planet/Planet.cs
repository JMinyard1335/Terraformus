using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



/// <summary>
/// Class that manages the planet on a high level.
/// </summary>
public class Planet : MonoBehaviour
{

    FaceGenerator faceGenerator;

    // How many triangles should be on each face  
    [Range(2, 256), Header("Planet Resolution")]
    public int resolution = 40;

    // Custom Settings Made from scriptable objects
    [Header("Custom Settings")]
    public ShapeSettings shapeSettings;
    public ColorSettings colorSettings;
    public TerrainSettings[] terrainSettings;

    [HideInInspector, SerializeField] MeshFilter[] meshFilters;
    PlanetFace[] planetFaces;

    // Used to allow the inspector menus to collapse
    [HideInInspector] public bool colorFoldout = true;
    [HideInInspector] public bool shapeFoldout = true;
    [HideInInspector] public bool[] terrainFoldouts;

    // Defines the directions of the normal Vectors for the faces.
    Vector3[] directions = {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    // Store normalized average elevation per face
    List<float> normalizedPerFace = new List<float>();

    void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            Debug.Log("Planet: MeshFilter Array was null or Empty. Creating a new one");
            meshFilters = new MeshFilter[6];
        }

        planetFaces = new PlanetFace[6];
        faceGenerator = new FaceGenerator(shapeSettings);

        Debug.Assert(faceGenerator != null, "Planet/Initialize: FaceGenerator was not created");
        Debug.Assert(meshFilters != null, "Planet/Initialize: MeshFilters was not created");
        Debug.Assert(planetFaces != null, "Planet/Initialize: PlanetFaces was not created");

        // Create the Meshes if they dont exist, and create the planet faces.
        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                CreateMeshObject(i);
            }

            planetFaces[i] = new PlanetFace(
                faceGenerator, meshFilters[i].sharedMesh,
                resolution, directions[i]
            );
        }
    }

    /// <summary>
    /// Used to create a Mesh Object
    /// Has a MeshFilter, MeshRenderer, and Mesh.
    /// </summary>
    /// <param name="i"></param>
    private void CreateMeshObject(int i)
    {
        Debug.Log("Planet/CreateMeshObject: MeshFilter [" + i + "] was null. Creating a new one");
        GameObject meshObj = new("mesh " + i);
        meshObj.transform.parent = transform;

        // Adds a meshRenderer which is used to display the mesh
        // Assign it the standard ShaderMaterial
        meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));

        // Give the object a MeshFilter which holds the mesh data, and assign it a new Mesh
        meshFilters[i] = meshObj.AddComponent<MeshFilter>();
        meshFilters[i].sharedMesh = new Mesh();

        // These Assertions are used to make sure the MeshObject was created and assigned.
        Debug.Assert(
            meshFilters[i] != null,
            "Planet/CreateMeshObject: MeshObject was not created."
        );
        Debug.Assert(
            meshFilters[i].sharedMesh != null,
            "Planet/CreateMeshObject: Mesh was not created."
        );

    }

    /// <summary>
    /// Called when the color settings change.
    /// Allows for dynamic updates from the editor
    /// </summary>
    public void OnColorChanged()
    {
        //Debug.Log("Color Settings Changed");
        Initialize();
        GenerateColors();
    }

    /// <summary>
    /// Called when the shape settings change.
    /// Allows for dynamic updates from the editor
    /// </summary>
    public void OnShapeChanged()
    {
        Debug.Log("Shape Settings Changed");
        Initialize();
        faceGenerator.UpdateNoise();
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

        //Calculate normalized average elevation per face
        List<float> allElevations = faceGenerator.elevationList;
        float minElevation = shapeSettings.noiseSettings.minValue;
        float maxElevation = allElevations.Max();

        normalizedPerFace.Clear();
        foreach (PlanetFace face in planetFaces)
        {
            float avg = face.faceElevations.Average();
            float norm = Mathf.InverseLerp(minElevation, maxElevation, avg);
            normalizedPerFace.Add(norm);

        }

        GenerateColors();
    }

    /// <summary>
    /// Generates the mesh for each PlanetFace.
    /// calls the ConstructMesh() method from the PlanetFace class.
    /// </summary>
    void GenerateMesh()
    {
        faceGenerator.ClearElevations();
        foreach (PlanetFace face in planetFaces)
        {
            face.ConstructMesh();
        }
    }

    /// <summary>
    /// TODO: Add a list to the FaceGenerator.cs script to hold the eleveation values
    ///     done - made a float list as part of the face generator class
    /// TODO: give that list to the planet face
    ///     done - basically done since the face generator is passed to the planet face
    /// TODO: Create a way to assign a float 0.0 -> 1.0 for different elevation ranges
    /// TODO: Set the float in the gradient shader based off of the previous step
    /// TODO: Change the float node in the shader to be referenceable 
    /// </summary>
    void GenerateColors()
    {
        for(int i=0; i < meshFilters.Length; i++)
        {
            float gradValue = i < normalizedPerFace.Count ? normalizedPerFace[i] : 0f;
            meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_GradHeight", gradValue);
        }
        
        
    }
}