# VoxelTerrainTest
First aproximation into making a voxel terrain in unity.

Project in which I experiment with the notion of voxels chunks and how to use them to build a 3d terrain. 

For the meshing, I'm using as a base the code written by Paul Bourke (poligonizing a scalar field), modified to work on c#.

To generate the terrain, I'm using Mathf.PerlinNoise() (Right now the terrain is generated using just a 2d scalar field. Later I plan to use perlin worms to generate caves and play around with a 3d scalar field).
