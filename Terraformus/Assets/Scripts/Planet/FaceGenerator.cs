using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Used to create the shape of the planet.
/// </summary>
public class FaceGenerator
{
    ShapeSettings shapeSettings;
    NoiseModule noiseModule;


    //Hannah added this line->
    //maybe not need to be public? w/e
    public List<float> elevationList;

    public FaceGenerator(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        noiseModule = new NoiseModule(shapeSettings.noiseSettings);
        //Hannah added this line->
        elevationList = new List<float>();
    }

    /// <summary>
    /// Calculates the position of a point on the planet based off of the noise.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 CalculatePosition(Vector3 pointOnSphere)
    {
        float elevation = noiseModule.Evaluate(pointOnSphere);
        //TODO: check if i am adding pure elevation or modified elevation???? to the list
        //Hannah added this line->
        elevationList.Add(elevation);
        return (1 + elevation) * shapeSettings.planetRadius * pointOnSphere;
    }

    public void UpdateNoise()
    {
        noiseModule.UpdateNoise();
    }

    /// <summary>
    /// normalizes the list of elevation values
    /// </summary>
    public List<float> GetNormalizedElevations()
    {
        float minElevation = shapeSettings.noiseSettings.minValue;
        float maxElevation = elevationList.Max();

        List<float> normalized = new List<float>();
        foreach(float elevation in elevationList)
        {
            float norm = Mathf.InverseLerp(minElevation, maxElevation, elevation);
            normalized.Add(norm);
        }

        return normalized;
    }

    /// <summary>
    /// clear the elevation list before each mesh generation
    /// </summary>
    public void ClearElevations()
    {
        elevationList.Clear();
    }
}
