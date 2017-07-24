using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class chunkManager : MonoBehaviour 
{
	
	public VoxelChunk chunk;
	private float isovalue;

	private MeshFilter mf;
	private MeshRenderer mr;
	private MeshCollider mc;

	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();



	// Use this for initialization
	public void StartChunkManager (VoxelChunk chunkInput) 
	{
//		this.mr = GetComponent<MeshRenderer> ();
//		this.mf = GetComponent<MeshFilter> ();
//		this.mc = GetComponent<MeshCollider> ();

		this.chunk = chunkInput;

		//Represents the offset to add to the vertices index to represent their real index:
		int offset = 0;

		//EXPERIMENT TO SEE WETHER THE VOXEL TO POLYGON WORKS:
		for (int y = 0; y < chunk.VoxelRes.y; y++)
		{
			for (int z = 0; z < chunk.VoxelRes.z; z++)
			{
				for (int x = 0; x < chunk.VoxelRes.x; x++)
				{
					GridCell cell = chunk.voxelGrid [x, y, z];
					MarchingCubes.Polygonise (cell, chunk.Isovalue, chunk.HalfInterpolation, ref vertices, ref triangles, ref offset);
				}
			}
		}

//		//Mesh configuration:
//		mf.mesh.SetVertices (vertices);
//		mf.mesh.SetTriangles (triangles, 0);
//
//		//Collision: Done for now:
//		mc.sharedMesh = null;
//		mc.sharedMesh = mf.mesh;
//
//		//missing the normals*** -> Later I'll use gradient to determine real values:
//		mf.mesh.RecalculateNormals();

		//Missing the uvs*** -> Triplanar shading (in progress)
		//TODO
	}



	// This is to separate the working the data out of the unity part of the code:
	public void SetUnityMesh () 
	{
		this.mr = GetComponent<MeshRenderer> ();
		this.mf = GetComponent<MeshFilter> ();
		this.mc = GetComponent<MeshCollider> ();

		//Mesh configuration:
		mf.mesh.SetVertices (vertices);
		mf.mesh.SetTriangles (triangles, 0);

		//Collision: Done for now:
		mc.sharedMesh = null;
		mc.sharedMesh = mf.mesh;

		//missing the normals*** -> Later I'll use gradient to determine real values:
		mf.mesh.RecalculateNormals();

		//Missing the uvs*** -> Triplanar shading (in progress)
		//TODO

		//Debug.Log ("Vertices has: " + vertices.Count + " elements.");
		//Debug.Log ("Triangles has: " + (triangles.Count / 3f) + " polygons.");
		//Debug.Log ("Now preparing to draw!");
	}



	//Threading experiment, erase if it doesn't solve issue:
	void cr1(VoxelChunk chunk, ref List<Vector3> vertices, ref List<int> triangles,  ref int offset)
	{
		//EXPERIMENT TO SEE WETHER THE VOXEL TO POLYGON WORKS:
		for (int y = 0; y < chunk.VoxelRes.y; y++)
		{
			for (int z = 0; z < chunk.VoxelRes.z; z++)
			{
				for (int x = 0; x < chunk.VoxelRes.x; x++)
				{
					GridCell cell = chunk.voxelGrid [x, y, z];

					MarchingCubes.Polygonise (cell, chunk.Isovalue, chunk.HalfInterpolation, ref vertices, ref triangles, ref offset);
				}
			}
		}
	}
}
