using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class chunkManager : MonoBehaviour 
{
	private MeshFilter mf;
	private MeshRenderer mr;
	private VoxelChunk chunk;

	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();


	// Use this for initialization
	void Start () 
	{
		this.mr = GetComponent<MeshRenderer> ();
		this.mf = GetComponent<MeshFilter> ();
		this.chunk = GetComponent<VoxelChunk> ();

		//This index will grow every pass to represent the extra vertices found:
		int vertexIndex = 0;

		//EXPERIMENT TO SEE WETHER THE VOXEL TO POLYGON WORKS:
		for (int y = 0; y < chunk.voxelRes; y++)
		{
			for (int z = 0; z < chunk.voxelRes; z++)
			{
				for (int x = 0; x < chunk.voxelRes; x++)
				{
					GridCell cell = chunk.voxelGrid [x, y, z];
					Triangle[] triArray = new Triangle[5];
					XYZ[] vertlist = new XYZ[12];

					int triangleAmount = MarchingCubes.Polygonise (cell, 0f, ref triArray, ref vertlist);

					/*
					for (int i = 0; i < 12; i++)
					{
						if (vertlist [i] != null)
							vertexIndex++;
					}

					for (int i = 0, v = 0; i < 12; i++)
					{
						if (vertlist [i] != null)
							vertices.Add (new Vector3(vertlist[i].x + ));
					}
					//*/
				}
			}
		}
	}
}
