using UnityEngine;

/// <summary>
/// Used to create the shape of the planet.
/// </summary>
public class FaceGenerator
{
    ShapeSettings shapeSettings;
    NoiseModule noiseModule;

    public FaceGenerator(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        noiseModule = new NoiseModule(shapeSettings.noiseSettings);
    }

    /// <summary>
    /// Calculates the position of a point on the planet based off of the noise.
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector3 CalculatePosition(Vector3 pointOnSphere)
    {
        float elevation = noiseModule.Evaluate(pointOnSphere);
        return (1 + elevation) * shapeSettings.planetRadius * pointOnSphere;
    }

    public void UpdateNoise()
    {
        noiseModule.UpdateNoise();
    }
}
