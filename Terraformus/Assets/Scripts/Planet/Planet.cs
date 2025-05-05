using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that manages the planet on a high level.
/// </summary>
public class Planet : MonoBehaviour
{

    FaceGenerator faceGenerator;
    public InputField inputField;
    public Slider resolutionSlider;

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

    void Start()
    {
        inputField.onEndEdit.AddListener(OnValueChanged);
        resolutionSlider.onValueChanged.AddListener(OnSliderChanged);
    }

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

    public void GenRandomPlanet()
    {
        shapeSettings.noiseSettings.seed = UnityEngine.Random.Range(0, 100000);
        GeneratePlanet();
    }

    void OnValueChanged(string input)
    {
        if (int.TryParse(input, out int result))
        {
            shapeSettings.noiseSettings.seed = result;
            GeneratePlanet();
        }
        else
        {
            Debug.LogWarning("Invalid input: not an integer");
        }
    }

    void OnSliderChanged(float value)
    {
        resolution = (int)value;
        GeneratePlanet();
    }


}