using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class represents a chunk (fixed size 3d grid) of voxels:
public class VoxelChunk : MonoBehaviour 
{
	//Size of the chunk in unity units:
	public float chunkSize = 5f;
	//number of voxels in each axis. For now, it will always be equal for all 3 axis:
	public int voxelRes = 5;
	//This represents the distance from one voxel cell to the next:
	public float voxelWidth;

	//Other useful data:
	public bool halfInterpolation = false;
	private float voxelHalfDist;
	private int cellCount;
	private int vertexCount;

	//Visual aid
	private LineRenderer[] lineArray;
	public bool seeBoundingBox = false;
	public bool seeOuterBounds = false;

	//The 3d array that contains each cell:
	public GridCell[,,] voxelGrid;



	// Use this for initialization
	void postStart (Vector2 position) // Awake()
	{
		//For now, I'm choosing to keep the voxels on a 3D array:
		voxelGrid = new GridCell[voxelRes, voxelRes, voxelRes];

		//VoxelHalfDist is the distance from the radius of the voxel to it's border. 
		//This number always should be a pot, even if it is less than one.
		voxelWidth = chunkSize / voxelRes;

		//Other useful values:
		voxelHalfDist = voxelWidth / 2f;
		cellCount = voxelRes * voxelRes * voxelRes;
		//vertexCount = (voxelRes + 1) * (voxelRes + 1) * (voxelRes + 1);

		//Visual aid:
		lineArray = new LineRenderer[cellCount];
		
		//Sets the grid with the voxel cells:
		setVoxelGrid (position);
	}


	//Set the vertices of the chunk. Starting always in origin:
	private void setVoxelGrid(Vector2 position)
	{
		for (int i = 0, y = 0; y < voxelRes; y++)
		{
			for (int z = 0; z < voxelRes; z++)
			{
				for (int x = 0; x < voxelRes; x++, i++)
				{
					//NOTE: how we pass the coords here determines how cells are ordered: **
					voxelGrid[x,y,z] = setVoxelCell (position, x, y, z);
				}
			}
		}
	}


	//Sets each gridCell with the corresponding values of each of its vertices:
	private GridCell setVoxelCell(Vector2 position, int x1, int y1, int z1)
	{
		float x = x1 * voxelWidth;
		float y = y1 * voxelWidth;
		float z = z1 * voxelWidth;

		//Data for the voxel  (center and interpolated value): NOT USED YET **
		Vector3 voxCenter = new Vector3 (x + voxelHalfDist, y + voxelHalfDist, z + voxelHalfDist);
		//float value = 0f;

		//First, we have to set every one of the vertices and their value: (CHECK VALUES)
		Vector3[] vertices = new Vector3[] {
			new Vector3 (x, y, z),
			new Vector3 (x + voxelWidth, y, z),
			new Vector3 (x + voxelWidth, y, z + voxelWidth),
			new Vector3 (x, y, z + voxelWidth),
			new Vector3 (x, y + voxelWidth, z),
			new Vector3 (x + voxelWidth, y + voxelWidth, z),
			new Vector3 (x + voxelWidth, y + voxelWidth, z + voxelWidth),
			new Vector3 (x, y + voxelWidth, z + voxelWidth)
		};


		//* 3D Noise terrains:
		float[] values = new float[] 
		{
//			(float)SimplexNoise.noise((x/chunkSize), (y / chunkSize), (z/chunkSize)),
//			(float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (y / chunkSize), (z/chunkSize)),
//			(float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (y / chunkSize), ((z + voxelWidth)/chunkSize)),
//			(float)SimplexNoise.noise((x/chunkSize), (y / chunkSize), ((z + voxelWidth)/chunkSize)),
//			(float)SimplexNoise.noise((x/chunkSize), ((y + voxelWidth) / chunkSize), (z/chunkSize)),
//			(float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), (z/chunkSize)),
//			(float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), ((z + voxelWidth)/chunkSize)),
//			(float)SimplexNoise.noise((x/chunkSize), ((y + voxelWidth) / chunkSize), ((z + voxelWidth)/chunkSize))

//			(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (y / chunkSize), (z/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (y / chunkSize), (z/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (y / chunkSize), ((z + voxelWidth)/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (y / chunkSize), ((z + voxelWidth)/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((y + voxelWidth) / chunkSize), (z/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), (z/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), ((z + voxelWidth)/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((y + voxelWidth) / chunkSize), ((z + voxelWidth)/chunkSize))

			(y / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			(y / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			(y / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			(y / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + (z/chunkSize)),
			((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + (z/chunkSize)),
			((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + ((z + voxelWidth)/chunkSize)),
			((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + ((z + voxelWidth)/chunkSize))
		};	
		//*/


		/* Using a 2d scalar field:
		float[] values = new float[] 
		{
			(y / chunkSize) * Mathf.PerlinNoise((x/chunkSize), (z/chunkSize)),
			(y / chunkSize) * Mathf.PerlinNoise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
			(y / chunkSize) * Mathf.PerlinNoise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
			(y / chunkSize) * Mathf.PerlinNoise((x/chunkSize), ((z + voxelWidth)/chunkSize)),
			((y + voxelWidth) / chunkSize) * Mathf.PerlinNoise((x/chunkSize), (z/chunkSize)),
			((y + voxelWidth) / chunkSize) * Mathf.PerlinNoise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
			((y + voxelWidth) / chunkSize) * Mathf.PerlinNoise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
			((y + voxelWidth) / chunkSize) * Mathf.PerlinNoise((x/chunkSize), ((z + voxelWidth)/chunkSize))

//			(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (z/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
//			(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((z + voxelWidth)/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (z/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
//			((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((z + voxelWidth)/chunkSize))
		};	
		//*/

		//Visual representation of the cell (for debugging purposes):
		if (seeBoundingBox)
			drawCellSpace(voxCenter, voxelHalfDist, y1 * voxelRes * voxelRes + z1 * voxelRes + x1);

		//Finally, we create the voxel cell and store it:
		return new GridCell (vertices, values);
	}


	//Debug method for visualizing the cells:
	private void drawCellSpace(Vector3 centerSpace, float halfSpaceLength, int index)
	{
		LineRenderer line = lineArray [index];
		GameObject obj = new GameObject ();

		obj.hideFlags = HideFlags.HideInHierarchy;
		line = obj.AddComponent<LineRenderer> ();
		line.useWorldSpace = true;
		line.SetWidth (0.02f, 0.02f);
		line.SetVertexCount (16);

		Vector3[] vertices = new Vector3[8];

		for(int y = 0; y < 2; y++)
		{
			for(int z = 0; z < 2; z++)
			{
				for(int x = 0; x < 2; x++)
				{
					Vector3 gen = new Vector3 (	centerSpace.x + halfSpaceLength * Mathf.Pow(-1f, x), 
						centerSpace.y + halfSpaceLength * Mathf.Pow(-1f, y), 
						centerSpace.z + halfSpaceLength * Mathf.Pow(-1f, z));
					vertices[4 * x + 2 * y + z] = gen;
				}
			}
		}

		line.SetPosition (0, vertices[0]);
		line.SetPosition (1, vertices[1]);
		line.SetPosition (2, vertices[3]);
		line.SetPosition (3, vertices[2]);
		line.SetPosition (4, vertices[0]);
		line.SetPosition (5, vertices[4]);
		line.SetPosition (6, vertices[5]);
		line.SetPosition (7, vertices[1]);
		line.SetPosition (8, vertices[5]);
		line.SetPosition (9, vertices[7]);
		line.SetPosition (10, vertices[3]);
		line.SetPosition (11, vertices[7]);
		line.SetPosition (12, vertices[6]);
		line.SetPosition (13, vertices[2]);
		line.SetPosition (14, vertices[6]);
		line.SetPosition (15, vertices[4]);
	}

}//EndOfClass: VoxelChunk-----//
