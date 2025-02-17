using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    [Header("Noise Movement")]
    public Vector3 center;

    [Header("Noise Settings")]
    [Range(0, 1)] public float strength = 1;
    [Range(0, 1)] public float roughness = 1;
    public int seed = 0;

    [Header("frequency Settings")]
    [Range(0, 2)] public float baseFrequency = 0.5f;
    [Range(1, 5)] public float mountainFrequency = 3f;

}
