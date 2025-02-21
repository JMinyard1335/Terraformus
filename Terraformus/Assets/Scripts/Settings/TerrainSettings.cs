using UnityEngine;

/// <summary>
/// Holds the independent settings for noise that change between Terrain types
/// </summary>
public class TNoiseSettings : ScriptableObject
{
    public float continentFrequency = 1;
    [Range(1, 30)] public int Octaves = 2;
    public float Prestiance = 0.5f;
    public float continentLacunarity = 2;

    /// <summary>
    /// Returns a value between 0 and 1 based off of the point.
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public virtual float Evaluate(Vector3 point)
    {
        return 0;
    }
}