using UnityEngine;
using LibNoise;
using LibNoise.Modifiers;

public class NoiseModule
{
    NoiseSettings settings;

    Blend finalModule;

    public NoiseModule(NoiseSettings settings)
    {
        this.settings = settings;
        NoiseSetup();
    }

    // Initalizes all the noise modules.
    private void NoiseSetup()
    {
        ClampOutput baseTerrain = CreateBaseTerrain();
        ClampOutput beaches = CreateBeaches();
        ClampOutput plains = CreatePlains();
        ClampOutput hills = CreateHills();
        ClampOutput baseMountains = CreateBaseMountains();
        ClampOutput tallMountains = CreateTallMountains();

        Perlin gradient = new Perlin();
        gradient.Seed = settings.seed + 20;
        gradient.Frequency = 0.01f;
        gradient.OctaveCount = 1;
        gradient.Lacunarity = 2;
        gradient.Persistence = 1;

        // Blend the base terrain with beaches
        var baseAndBeaches = new Blend(baseTerrain, beaches, gradient);

        // Blend the result with plains
        var baseBeachesAndPlains = new Blend(baseAndBeaches, plains, gradient);

        // Blend the result with hills
        var baseBeachesPlainsAndHills = new Blend(baseBeachesAndPlains, hills, gradient);

        // Blend the result with base mountains
        var baseBeachesPlainsHillsAndMountains = new Blend(baseBeachesPlainsAndHills, baseMountains, gradient);

        // Blend the result with tall mountains
        finalModule = new Blend(baseBeachesPlainsHillsAndMountains, tallMountains, gradient);

    }

    private ClampOutput CreateBaseTerrain()
    {
        var baseTerrain = new Perlin();
        baseTerrain.Seed = settings.seed;
        baseTerrain.Frequency = settings.continentFrequency;
        baseTerrain.OctaveCount = settings.continentOctaves;
        baseTerrain.Persistence = settings.continentPrestiance;
        baseTerrain.Lacunarity = settings.continentLacunarity;

        // Make sure the base ground stays below a certain height.
        var clampedTerrain = new ClampOutput(baseTerrain);
        clampedTerrain.SetBounds(0, settings.baseTerrainHeight);
        return clampedTerrain;
    }

    private ClampOutput CreateBeaches()
    {
        // Add beaches to the base terrain.
        var beaches = new Billow();
        beaches.Seed = settings.seed + 5;
        beaches.Frequency = settings.beachFrequency;
        beaches.OctaveCount = settings.beachOctaves;
        beaches.Lacunarity = settings.beachLacunarity;
        beaches.Persistence = settings.beachPersistence;

        // Make sure the beaches stay below a certain height and above the ground.
        var clampedBeaches = new ClampOutput(beaches);
        clampedBeaches.SetBounds(0, settings.beachHeight);
        return clampedBeaches;
    }

    private ClampOutput CreatePlains()
    {
        // Add plains to the base terrain.
        var plains = new Billow();
        plains.Seed = settings.seed + 10;
        plains.Frequency = settings.plainsFrequency;
        plains.OctaveCount = settings.plainsOctaves;
        plains.Lacunarity = settings.plainsLacunarity;
        plains.Persistence = settings.plainsPersistence;

        // Make sure the plains stay below a certain height and above the ground.
        var clampedPlains = new ClampOutput(plains);
        clampedPlains.SetBounds(0, settings.plainsHeight);
        return clampedPlains;
    }

    private ClampOutput CreateHills()
    {
        // Add hills to the base terrain.
        var hills = new Billow();
        hills.Seed = settings.seed + 15;
        hills.Frequency = settings.hillFrequency;
        hills.OctaveCount = settings.hillOctaves;
        hills.Lacunarity = settings.hillLacunarity;
        hills.Persistence = settings.hillPersistence;

        // Make sure the hills stay below a certain height and above the ground.
        var clampedHills = new ClampOutput(hills);
        clampedHills.SetBounds(0, settings.hillsHeight);
        return clampedHills;
    }

    private ClampOutput CreateBaseMountains()
    {
        // Add mountains to the hills.
        var mountains = new Perlin();
        mountains.Seed = settings.seed + 30;
        mountains.Frequency = settings.mountainFrequency;
        mountains.OctaveCount = settings.mountainOctaves;
        mountains.Lacunarity = settings.mountainLacunarity;
        mountains.Persistence = settings.mountainPersistence;

        // Make sure the mountains stay below a certain height and above the hills.
        var clampedMountains = new ClampOutput(mountains);
        clampedMountains.SetBounds(settings.mountainsBaseHeight, settings.mountainsHeight);
        return clampedMountains;
    }

    private ClampOutput CreateTallMountains()
    {
        // Add mountains to the hills.
        var mountains = new Perlin();
        mountains.Seed = settings.seed + 40;
        mountains.Frequency = settings.mountainFrequency;
        mountains.OctaveCount = settings.mountainOctaves;
        mountains.Lacunarity = settings.mountainLacunarity;
        mountains.Persistence = settings.mountainPersistence;

        // Make sure the mountains stay below a certain height and above the hills.
        var clampedMountains = new ClampOutput(mountains);
        clampedMountains.SetBounds(settings.mountainsBaseHeight, settings.mountainsHeight);
        return clampedMountains;
    }

    public float Evaluate(Vector3 point)
    {
        float value = (float)finalModule.GetValue(point.x, point.y, point.z);
        value = Mathf.Max(0, value - settings.minValue);
        return Mathf.Max(0, value);
    }
}