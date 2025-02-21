using UnityEngine;
using LibNoise;
using LibNoise.Modifiers;

public class NoiseModule
{
    #region Attributes
    NoiseSettings settings;

    // Define the Terrain types to use in the noise module.
    BaseTerrain baseTerrain;
    Plains plains;
    Hills hills;
    Beaches beaches;
    LowMountains lowMountains;
    TallMountains tallMountains;

    // This will be a combination of the above terrain types.
    Blend finalModule;
    #endregion

    #region Constructors
    public NoiseModule(NoiseSettings settings)
    {
        this.settings = settings;

        baseTerrain = new BaseTerrain(settings);
        beaches = new Beaches(settings);
        plains = new Plains(settings);
        hills = new Hills(settings);
        lowMountains = new LowMountains(settings);
        tallMountains = new TallMountains(settings);

        NoiseSetup();
    }
    #endregion


    #region Methods
    // Initalizes all the noise modules.
    private void NoiseSetup()
    {
        Perlin gradient = new()
        {
            Seed = settings.seed + 20,
            Frequency = 0.01f,
            OctaveCount = 1,
            Lacunarity = 2,
            Persistence = 1
        };

        // Blend the base terrain with beaches
        Blend baseAndBeaches = new(baseTerrain.GetTerrain(), beaches.GetTerrain(), gradient);

        // Blend the result with plains
        Blend baseBeachesAndPlains = new(baseAndBeaches, plains.GetTerrain(), gradient);

        // Blend the result with hills
        Blend baseBeachesPlainsAndHills = new(baseBeachesAndPlains, hills.GetTerrain(), gradient);

        // Blend the result with base mountains
        Blend baseBeachesPlainsHillsAndMountains = new(baseBeachesPlainsAndHills, lowMountains.GetTerrain(), gradient);

        // Blend the result with tall mountains
        finalModule = new Blend(
            baseBeachesPlainsHillsAndMountains,
            tallMountains.GetTerrain(),
            gradient
        );
    }

    public float Evaluate(Vector3 point)
    {
        float value = (float)finalModule.GetValue(point.x, point.y, point.z);
        value = Mathf.Max(0, value - settings.minValue);
        return Mathf.Max(0, value);
    }

    public void UpdateNoise()
    {
        NoiseSetup();
    }

    #endregion
}