using UnityEngine;
using LibNoise;
using LibNoise.Generator;


public class ShapeSpawner 
{
    ShapeSettings shapeSettings;
    NoiseManager noiseManager;

    public ShapeSpawner(ShapeSettings shapeSettings)
    {
        this.shapeSettings = shapeSettings;
        noiseManager = new NoiseManager(shapeSettings.noiseSettings);
    }

    public Vector3 CalculatePosition(Vector3 pos)
    {
        float elevation = noiseManager.Evaluate(pos);
        return pos * shapeSettings.planetRadius * (1+elevation);
    }
}
