using UnityEngine;
using LibNoise;
using LibNoise.Modifiers;

public class Hills : Terrain
{
    public Hills(NoiseSettings nSettings)
    {
        tSettings = Resources.Load<TerrainSettings>("HillsSettings");
        this.nSettings = nSettings;
        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        // Add hills to the base terrain.
        Billow baseHills = new()
        {
            Seed = nSettings.seed + 15,
            Frequency = tSettings.frequency,
            OctaveCount = tSettings.octaves,
            Lacunarity = tSettings.lacunarity,
            Persistence = tSettings.persistence
        };

        // Make sure the hills stay below a certain height and above the ground.
        _terrain = new ClampOutput(baseHills);
        _terrain.SetBounds(0, nSettings.hillsHeight);
    }
}