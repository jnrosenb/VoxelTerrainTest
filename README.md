# VoxelTerrainTest
# First aproximation into making a voxel terrain in unity.

Project in which I experiment with the notion of voxels chunks and how to use them to build a 3d terrain. 

For the Polygonizing, I'm using as a base the code written by Paul Bourke (poligonizing a scalar field), modified to work on c# and changed to fit with unity's mesh creation process.

For the 3d terrains I'm using 3d simplex noise, a c# implementation based on stefan Gustavsen code explained in this article: 
(http://staffwww.itn.liu.se/~stegu/simplexnoise/simplexnoise.pdf)

Right now, you can play with the voxelMap prefab and choose a radius (which will determine how many chunks of terrain it creates), the isovalue, and the size of each chunk in x axis, y axis and z axis.

Also, if you check the chunk loading checkbox, it will create new chunks as you move in realtime.


![alt text](http://i.imgur.com/bOBKCCT.png)


![alt text](http://i.imgur.com/XNx8IhH.png)


![alt text](http://i.imgur.com/1ECfdFa.png)
