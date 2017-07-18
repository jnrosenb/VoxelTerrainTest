using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//This class represents a chunk (fixed size 3d grid) of voxels:
public class VoxelChunk : MonoBehaviour 
{
	//Chunk configuration:
	private bool halfInterpolation;
	private Vector3 chunkSize;
	private Vector3 voxelRes;
	private float voxelWidth;
	private float isovalue;

	//Propoerties:
	public bool HalfInterpolation{ get{ return halfInterpolation; } }
	public Vector3 VoxelRes{ get{ return voxelRes; } }
	public float Isovalue{ get{ return isovalue; } }

	//Other useful data:
	private float voxelHalfDist;
	private int cellCount;
	private int vertexCount;

	//Visual aid
	private LineRenderer[] lineArray;
	public bool seeBoundingBox = false;
	public bool seeOuterBounds = false;

	//The 3d array that contains each cell:
	public GridCell[,,] voxelGrid;



	// ARGS: [0]: starting pos offset, [1]: chunkSize, [2]: VoxelRes, [3]: Isovalue, [4]: halfInterpolation, [5]: voxelWidth.
	public void postStart (List<System.Object> args) // Awake()
	{
		//Unpacking data from the args:
		Vector2 startingPositionOffset = (Vector2)args[0];
		chunkSize = (Vector3)args[1];
		voxelRes = (Vector3)args [2];
		isovalue = (float)args [3];
		halfInterpolation = (bool)args [4];
		voxelWidth = (float)args [5];

		//Voxels are going to be kept on a 3d array:
		voxelGrid = new GridCell[(int)voxelRes.x, (int)voxelRes.y, (int)voxelRes.z];

		//Other useful values:
		voxelHalfDist = voxelWidth / 2f;
		cellCount = (int)(voxelRes.x * voxelRes.y * voxelRes.z);
		vertexCount = (int)((voxelRes.x + 1) * (voxelRes.y + 1) * (voxelRes.z + 1));

		//Visual aid:
		lineArray = new LineRenderer[cellCount];
		
		//Sets the grid with the voxel cells:
		setVoxelGrid (startingPositionOffset);
	}


	//Set the vertices of the chunk. Starting always in origin:
	private void setVoxelGrid(Vector2 position)
	{
		for (int i = 0, y = 0; y < voxelRes.y; y++)
		{ 
			for (int z = 0; z < voxelRes.z; z++)
			{
				for (int x = 0; x < voxelRes.x; x++, i++)
				{
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

		//First, we have to set every one of the vertices and their value: (CHECK VALUES)
		Vector3[] vertices = new Vector3[] {

			//The (-chunkSize/2f) term is there so that the mesh has its pivot in the center of the chunk (with y = 0).
			new Vector3 (x - chunkSize.x/2f, y, z - chunkSize.z/2f),
			new Vector3 (x - chunkSize.x/2f + voxelWidth, y, z - chunkSize.z/2f),
			new Vector3 (x - chunkSize.x/2f + voxelWidth, y, z - chunkSize.z/2f + voxelWidth),
			new Vector3 (x - chunkSize.x/2f, y, z - chunkSize.z/2f + voxelWidth),
			new Vector3 (x - chunkSize.x/2f, y + voxelWidth, z - chunkSize.z/2f),
			new Vector3 (x - chunkSize.x/2f + voxelWidth, y + voxelWidth, z - chunkSize.z/2f),
			new Vector3 (x - chunkSize.x/2f + voxelWidth, y + voxelWidth, z - chunkSize.z/2f + voxelWidth),
			new Vector3 (x - chunkSize.x/2f, y + voxelWidth, z - chunkSize.z/2f + voxelWidth)
		};


		//* 3D Noise terrains:
		float[] values = new float[] 
		{
//			(float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, (y / chunkSize.x)					, position.y + (z/chunkSize.z)) ,
//			(float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), (y / chunkSize.x)					, position.y + (z/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), (y / chunkSize.x)					, position.y + ((z + voxelWidth)/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, (y / chunkSize.x)					, position.y + ((z + voxelWidth)/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, ((y + voxelWidth) / chunkSize.x)	, position.y + (z/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), ((y + voxelWidth) / chunkSize.x)	, position.y + (z/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), ((y + voxelWidth) / chunkSize.x)	, position.y + ((z + voxelWidth)/chunkSize.z)),
//			(float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, ((y + voxelWidth) / chunkSize.x)	, position.y + ((z + voxelWidth)/chunkSize.z))

			//(y / chunkSize) * (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize), position.y + (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), position.y + (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize), position.y + ((z + voxelWidth)/chunkSize))

			//(y / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			//(y / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + (z/chunkSize)),
			//(y / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			//(y / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), (y / chunkSize), position.y + ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + ((x + voxelWidth)/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) + (float)SimplexNoise.noise(position.x + (x/chunkSize), ((y + voxelWidth) / chunkSize),  position.y + ((z + voxelWidth)/chunkSize))
		
			8*(y  / chunkSize.y) 				+ (float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, (y / chunkSize.y)					, position.y + (z/chunkSize.z)) 				- 0.6f,
			8*(y  / chunkSize.y) 				+ (float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), (y / chunkSize.y)					, position.y + (z/chunkSize.z))					- 0.6f,
			8*(y  / chunkSize.y) 				+ (float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), (y / chunkSize.y)					, position.y + ((z + voxelWidth)/chunkSize.z))	- 0.6f,
			8*(y  / chunkSize.y) 				+ (float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, (y / chunkSize.y)					, position.y + ((z + voxelWidth)/chunkSize.z))	- 0.6f,
			8*((y  + voxelWidth) / chunkSize.y) + (float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, ((y + voxelWidth) / chunkSize.y)	, position.y + (z/chunkSize.z))					- 0.6f,
			8*((y  + voxelWidth) / chunkSize.y) + (float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), ((y + voxelWidth) / chunkSize.y)	, position.y + (z/chunkSize.z)) 				- 0.6f,
			8*((y  + voxelWidth) / chunkSize.y) + (float)SimplexNoise.noise (position.x + ((x + voxelWidth)/chunkSize.x), ((y + voxelWidth) / chunkSize.y)	, position.y + ((z + voxelWidth)/chunkSize.z)) 	- 0.6f,
			8*((y  + voxelWidth) / chunkSize.y) + (float)SimplexNoise.noise (position.x + (x/chunkSize.x)				, ((y + voxelWidth) / chunkSize.y)	, position.y + ((z + voxelWidth)/chunkSize.z)) 	- 0.6f
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

			//(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (z/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
			//(y / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), (z/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise(((x + voxelWidth)/chunkSize), ((z + voxelWidth)/chunkSize)),
			//((y + voxelWidth) / chunkSize) * (float)SimplexNoise.noise((x/chunkSize), ((z + voxelWidth)/chunkSize))
		};	
		//*/

		//Visual representation of the cell (for debugging purposes):
		//if (seeBoundingBox)
			//drawCellSpace(voxCenter, voxelHalfDist, y1 * voxelRes * voxelRes + z1 * voxelRes + x1);

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
