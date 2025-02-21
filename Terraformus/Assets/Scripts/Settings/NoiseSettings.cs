using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public int seed;

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