using LibNoise;
using LibNoise.Modifiers;
using UnityEngine;

public class BaseTerrain : Terrain
{
    public BaseTerrain(NoiseSettings nSettings)
    {
        tSettings = Resources.Load<TerrainSettings>("BaseTerrainSettings");
        if (tSettings == null)
        {
            Debug.LogError("BaseTerrainSettings not found.");
        }

        this.nSettings = nSettings;
        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        Perlin noise = new()
        {
            Seed = nSettings.seed,
            Frequency = tSettings.frequency,
            OctaveCount = tSettings.octaves,
            Persistence = tSettings.persistence,
            Lacunarity = tSettings.lacunarity
        };

        // Make sure the base ground stays below a certain height.
        _terrain = new ClampOutput(noise);
        _terrain.SetBounds(0, nSettings.baseTerrainHeight);
    }
}