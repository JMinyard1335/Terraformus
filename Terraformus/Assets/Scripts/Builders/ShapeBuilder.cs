using UnityEngine;

/// <summary>
/// Used to create the shape of the planet.
/// </summary>
public class ShapeBuilder
{
    ShapeSettings shapeSettings;
    NoiseModule noiseModule;

    public ShapeBuilder(ShapeSettings shapeSettings, ColorSettings colorSettings)
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
}
