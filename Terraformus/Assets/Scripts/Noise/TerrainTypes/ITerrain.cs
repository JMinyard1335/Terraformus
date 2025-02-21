public interface ITerrain
{
    // The TerrainSettings define the limits of the noise.
    TerrainSettings tSettings { get; set; }
    NoiseSettings nSettings { get; set; }

    // Should be used to implement that terrains noise module.
    // The results can then be combined with other terrains.
    public void ConstructNoise();
}