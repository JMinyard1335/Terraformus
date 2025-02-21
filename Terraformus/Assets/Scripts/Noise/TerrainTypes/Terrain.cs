using LibNoise.Modifiers;
using LibNoise;
using UnityEngine;

public abstract class Terrain : ITerrain
{

    private TerrainSettings _tSettings;
    public TerrainSettings tSettings
    {
        get => _tSettings;
        set => _tSettings = value;
    }

    private NoiseSettings _nSettings;
    public NoiseSettings nSettings
    {
        get => _nSettings;
        set => _nSettings = value;
    }

    protected ClampOutput _terrain;
    public ClampOutput GetTerrain()
    {
        if (_terrain == null)
        {
            Debug.LogError("Terrain not created");
        }

        return _terrain;
    }

    public abstract void ConstructNoise();
}