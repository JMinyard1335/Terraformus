# Terraformus
Terraformus will be a 3D space experience. In it the user will get to see and be a part of a world evolving over time. Your rock changes over time as mighty mountains rise, and the raging rivers cut winding caverns through the landscape. Grass begins to grow, then the trees take hold towering over the landscape. All of this while the user can affect the ways in which the planet evolves.

![EarlyTerrainGen](https://github.com/user-attachments/assets/74936765-9eda-4af7-8f7d-da50fbb5974f)


# Tech
- [Unity 6](https://unity.com/releases/unity-6)
- [libNoise](https://libnoise.sourceforge.net/)
- [CalmBit.LibNoise](https://github.com/CalmBit/LibNoise)

# Project Goals:
## Functional Minimum: 
- Simple Procedural Planet Generation.
	- Mountains
	- Grasslands
	- Ocean
- Simple User controls
	- The user should be able to spin the planet.
	- The user should be able to change the zoom level.
		
## Minimum Viable Product:
- Let the planet change over time.
  - Starts off smoother
  - Gains some Sharp Mountains
  - Mountains become weathered and smooth
- Spawn Planet level details
  - trees
  - rivers
		
## Desired 
- Allow the user to change features on the planet
  - Plant Trees
  - Add rivers
  - Change Height

## Additional 
- Add additional Biomes
  - Tundra
  - Canyons
  - Desert

# Building The Planet
The planet is created by creating a grid of vertices based off of the desired resolution. The squares that then make up the grid are divided in to two triangles. 6 Grids are created and arranged in a cube. Each point from these faces on the unit cube are then pushed to round out the cube. Height is then applied to the point based off of the Noise. 

# Terrain Generation
Procedural terrain generation uses coherent noise which is a type of visual noise with the following key properties
- same input gives the same output (Giving the same input should not result in different outputs)
- Small changes to the input result in small changes to the output
- Large changes in the input result in Large/Random Changes in the output.

Using [LibNoise](https://github.com/CalmBit/LibNoise) I created different types of noise to represent different terrain features.
These features clamped to a specific height to keep different types of terrains to different elevations in the final terrain.
The features are then blended together to create the final noise function.

Different terrain types have been defined that can be combined in different ways to generate unique combinations.
The current terrains implemented:
- BaseTerrain
- Beaches
- Plains
- Hills
- LowMountains
- TallMountains

Each of the above types have there own Scriptable Object Settings. 
When customizing the settings in the editor some guidelines that seem to produce good terrain are the following
- Lowlands
  - High Frequency
  - High Octave Count
- Higher Terrains or Terrains that should be rarer
  - Low Frequency
  - Low Octave Count
  

