using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public int seed;
    [Header("Continent Settings")]
    public float continentFrequency = 1;
    [Range(1, 30)] public int continentOctaves = 1;
    public float continentPrestiance = 1;
    public float continentLacunarity = 2;

    [Header("Beach Settings")]
    public float beachFrequency = 1;
    [Range(1, 30)] public int beachOctaves = 1;
    public float beachPersistence = 1;
    public float beachLacunarity = 2;

    [Header("Plains Settings")]
    public float plainsFrequency = 1;
    [Range(1, 30)] public int plainsOctaves = 1;
    public float plainsPersistence = 1;
    public float plainsLacunarity = 2;

    [Header("Hill Settings")]
    public float hillFrequency = 1;
    [Range(1, 30)] public int hillOctaves = 1;
    public float hillPersistence = 1;
    public float hillLacunarity = 2;

    [Header("Mountain Base Settings")]
    public float mountainBaseFrequency = 1;
    [Range(1, 30)] public int mountainBaseOctaves = 1;
    public float mountainBasePersistence = 1;
    public float mountainBaseLacunarity = 2;

    [Header("Mountain Settings")]
    public float mountainFrequency = 1;
    [Range(1, 30)] public int mountainOctaves = 1;
    public float mountainPersistence = 1;
    public float mountainLacunarity = 2;

    [Header("Region Height Restrictions")]
    // base terrain can go as high as 0.03
    public float baseTerrainHeight = 0.1f;
    public float beachHeight = 0.2f;
    public float plainsHeight = 0.3f;
    public float hillsHeight = 0.4f;
    public float mountainsBaseHeight = 0.5f;
    public float mountainsHeight = 0.7f;

    [Header("Cut off Value"), Range(0, 1)]
    public float minValue = 0;

}