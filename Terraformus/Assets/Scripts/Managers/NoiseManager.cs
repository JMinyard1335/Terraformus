using UnityEngine;
using LibNoise;
using LibNoise.Generator;
using LibNoise.Operator;

public class NoiseManager
{
    ModuleBase noise;
    NoiseSettings settings;


    public NoiseManager(NoiseSettings settings)
    {
        this.settings = settings;
        LetsMakeSomeNoise();
    }

    public void LetsMakeSomeNoise()
    {
        Perlin baseTerrain = CreateBaseTerrain();
        RidgedMultifractal mountains = CreateMountains();
        Billow hills = CreateHills();
        Select hillsAndMountians = BlendTerrain(hills, mountains);
        Select terrainSelector = new Select(
            baseTerrain,
            hillsAndMountians,
            new Perlin(0.5, 2.0, 0.5, 3, settings.seed + 100, QualityMode.Medium)
        );

        noise = ApplyTurbulence(terrainSelector);
    }

    public float Evaluate(Vector3 point)
    {
        float x = point.x * settings.roughness + settings.center.x;
        float y = point.y * settings.roughness + settings.center.y;
        float z = point.z * settings.roughness + settings.center.z;
        float noiseValue = (float)noise.GetValue(x, y, z);
        noiseValue = Mathf.Clamp(noiseValue, -0.8f, 0.8f);
        return (noiseValue + 1) * 0.5f * settings.strength;
    }

    #region Terrain Creation Functions

    private Perlin CreateBaseTerrain()
    {
        return new Perlin(
            frequency: settings.baseFrequency,
            lacunarity: 2.0,
            persistence: 0.5,
            octaves: 6,
            seed: settings.seed,
            quality: QualityMode.Medium
        );
    }

    private RidgedMultifractal CreateMountains()
    {
        // Create ridges and mountains using RidgedMultifractal
        return new RidgedMultifractal(
            frequency: settings.mountainFrequency,
            lacunarity: 2.2,
            octaves: 4,
            seed: settings.seed + 1,
            quality: QualityMode.Medium
        );
    }

    private Billow CreateHills()
    {
        return new Billow(
            settings.baseFrequency * 2,
            2.0,
            0.5,
            3,
            settings.seed + 200,
            QualityMode.Medium
        );
    }

    private Select BlendTerrain(ModuleBase terrainA, ModuleBase terrainB)
    {
        return new Select(
            terrainA,
            terrainB,
            new Perlin(0.5, 2.0, 0.5, 3, settings.seed + 100, QualityMode.Medium)
        );
    }

    private ModuleBase ApplyTurbulence(ModuleBase terrain)
    {
        return new Turbulence(
            0.5,
            terrain
        );
    }


    #endregion
}
