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
	private float voxelWidth;

	//Other useful data:
	private float voxelHalfDist;
	private int cellCount;
	private int vertexCount;

	//The 3d array that contains each cell:
	public GridCell[,,] voxelGrid;


	// Use this for initialization
	void Awake () 
	{
		//For now, I'm choosing to keep the voxels on a 3D array:
		voxelGrid = new GridCell[voxelRes, voxelRes, voxelRes];

		//VoxelHalfDist is the distance from the radius of the voxel to it's border. 
		//This number always should be a pot, even if it is less than one.
		voxelWidth = chunkSize / voxelRes;

		//Other useful values:
		voxelHalfDist = voxelWidth / 2f;
		cellCount = voxelRes * voxelRes * voxelRes;
		vertexCount = (voxelRes + 1) * (voxelRes + 1) * (voxelRes + 1);

		//Sets the grid with the voxel cells:
		setVoxelGrid ();

		Debug.Log ("Grid ready and set!");
	}


	//Set the vertices of the chunk. Starting always in origin:
	private void setVoxelGrid()
	{
		for (int i = 0, y = 0; y < voxelRes; y++)
		{
			for (int z = 0; z < voxelRes; z++)
			{
				for (int x = 0; x < voxelRes; x++, i++)
				{
					//NOTE: how we pass the coords here determines how cells are ordered: **
					voxelGrid[x,y,z] = setVoxelCell (x, y, z);
				}
			}
		}
	}


	//Sets each gridCell with the corresponding values of each of its vertices:
	private GridCell setVoxelCell(int x1, int y1, int z1)
	{
		float x = x1 * voxelWidth;
		float y = y1 * voxelWidth;
		float z = z1 * voxelWidth;

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

		//Here we set the value for each vertex (temporal):
		float[] values = new float[] 
		{
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//			Random.Range(-1f, y/voxelRes),
//		    Random.Range(-1f, y/voxelRes)	
			(float)SimplexNoise.noise(x, y, z),
			(float)SimplexNoise.noise(x + voxelWidth, y, z),
			(float)SimplexNoise.noise(x + voxelWidth, y, z + voxelWidth),
			(float)SimplexNoise.noise(x, y, z + voxelWidth),
			(float)SimplexNoise.noise(x, y + voxelWidth, z),
			(float)SimplexNoise.noise(x + voxelWidth, y + voxelWidth, z),
			(float)SimplexNoise.noise(x + voxelWidth, y + voxelWidth, z + voxelWidth),
			(float)SimplexNoise.noise(x, y + voxelWidth, z + voxelWidth)
		};
			
		//Data for the voxel  (center and interpolated value): NOT USED YET **
		Vector3 voxCenter = new Vector3 (x + voxelHalfDist, y + voxelHalfDist, z + voxelHalfDist);
		float value = 0f;

		//Finally, we create the voxel cell and store it:
		return new GridCell (vertices, values);
	}


	//This method returns the noise at the given coords. Right now it is just a random call:
	private float getValue(int x, int y, int z)
	{
		return Random.Range (0f, 1f);
	}


}//EndOfClass: VoxelChunk-----//
