using UnityEngine;
using LibNoise;
using LibNoise.Modifiers;

public class Beaches : Terrain
{
    public Beaches(NoiseSettings nSettings)
    {
        tSettings = Resources.Load<TerrainSettings>("BeachSettings");
        if (tSettings == null)
        {
            Debug.LogError("BeachSettings not found.");
        }

        this.nSettings = nSettings;
        ConstructNoise();
    }

    public override void ConstructNoise()
    {
        Billow baseBeaches = new()
        {
            Seed = nSettings.seed + 5,
            Frequency = tSettings.frequency,
            OctaveCount = tSettings.octaves,
            Lacunarity = tSettings.lacunarity,
            Persistence = tSettings.persistence
        };

        // Make sure the beaches stay below a certain height and above the ground.
        _terrain = new ClampOutput(baseBeaches);
        _terrain.SetBounds(0, nSettings.beachHeight);
    }

}