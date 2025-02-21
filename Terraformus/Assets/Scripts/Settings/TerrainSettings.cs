using UnityEngine;

/// <summary>
/// Holds the independent settings for noise that change between Terrain types
/// That includes the frequency, octaves, and prestiance.
/// </summary>
[CreateAssetMenu()]
public class TerrainSettings : ScriptableObject
{
    public float frequency = 1;
    [Range(1, 30)] public int octaves = 2;
    public float persistence = 0.5f;
    public float lacunarity = 2;
}