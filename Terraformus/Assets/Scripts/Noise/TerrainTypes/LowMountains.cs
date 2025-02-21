using UnityEngine;
using LibNoise;
using LibNoise.Modifiers;

public class LowMountains : Terrain
{
    public LowMountains(NoiseSettings nSettings)
    {
        this.nSettings = nSettings;
        tSettings = Resources.Load<TerrainSettings>("LowMountainSettings");
        if (tSettings == null)
        {
            Debug.LogError("LowMountainSettings not found");
        }

        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        Perlin mountains = new()
        {
            Seed = nSettings.seed + 30,
            Frequency = tSettings.frequency,
            OctaveCount = tSettings.octaves,
            Lacunarity = tSettings.lacunarity,
            Persistence = tSettings.persistence
        };

        // Make sure the mountains stay below a certain height and above the hills.
        _terrain = new ClampOutput(mountains);
        _terrain.SetBounds(nSettings.mountainsBaseHeight, nSettings.mountainsHeight);
    }
}