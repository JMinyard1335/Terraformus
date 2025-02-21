using LibNoise;
using LibNoise.Modifiers;
using UnityEngine;

public class TallMountains : Terrain
{
    public TallMountains(NoiseSettings nSettings)
    {
        this.nSettings = nSettings;
        tSettings = Resources.Load<TerrainSettings>("TallMountainSettings");
        if (tSettings == null)
        {
            Debug.LogError("TallMountainSettings not found");
        }

        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        // Add mountains to the hills.
        Perlin mountains = new()
        {
            Seed = nSettings.seed + 40,
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
