using LibNoise;
using LibNoise.Modifiers;
using UnityEngine;

public class Plains : Terrain
{
    public Plains(NoiseSettings nSettings)
    {
        tSettings = Resources.Load<TerrainSettings>("PlainsSettings");
        if (tSettings == null)
        {
            Debug.LogError("PlainsSettings not found.");
        }

        this.nSettings = nSettings;
        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        Billow basePlains = new Billow
        {
            Seed = nSettings.seed + 10,
            Frequency = tSettings.frequency,
            OctaveCount = tSettings.octaves,
            Lacunarity = tSettings.lacunarity,
            Persistence = tSettings.persistence
        };

        // Make sure the plains stay below a certain height and above the ground.
        _terrain = new ClampOutput(basePlains);
        _terrain.SetBounds(0, nSettings.plainsHeight);
    }


}