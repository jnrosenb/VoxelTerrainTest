using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class chunkManager : MonoBehaviour 
{
	[Range(-1.0f, 1.0f)]
	public float isovalue = 0.0f;

	private MeshFilter mf;
	private MeshRenderer mr;
	private MeshCollider mc;
	private VoxelChunk chunk;

	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();


	// Use this for initialization
	void Start () 
	{
		this.mr = GetComponent<MeshRenderer> ();
		this.mf = GetComponent<MeshFilter> ();
		this.mc = GetComponent<MeshCollider> ();
		this.chunk = GetComponent<VoxelChunk> ();

		mr.material = new Material (Shader.Find("Standard"));
		mr.material.color = Color.white;

		//Represents the offset to add to the vertices index to represent their real index:
		int offset = 0;

		//EXPERIMENT TO SEE WETHER THE VOXEL TO POLYGON WORKS:
		for (int y = 0; y < chunk.voxelRes; y++)
		{
			for (int z = 0; z < chunk.voxelRes; z++)
			{
				for (int x = 0; x < chunk.voxelRes; x++)
				{
					GridCell cell = chunk.voxelGrid [x, y, z];
					Triangle[] triArray = new Triangle[] 
					{
						new Triangle(new Vector3[3]),
						new Triangle(new Vector3[3]),
						new Triangle(new Vector3[3]),
						new Triangle(new Vector3[3]),
						new Triangle(new Vector3[3])		
					};

					//isovalue = Mathf.PerlinNoise ((x * chunk.voxelWidth / chunk.chunkSize), (z * chunk.voxelWidth / chunk.chunkSize));
					MarchingCubes.Polygonise (cell, isovalue, chunk.halfInterpolation, ref triArray, ref vertices, ref triangles, ref offset);// ***EXPERIMENT*** 

				}
			}
		}

		Debug.Log ("Vertices has: " + vertices.Count + " elements.");
		Debug.Log ("Triangles has: " + (triangles.Count / 3f) + " polygons.");
		Debug.Log ("Now preparing to draw!");

		mf.mesh.SetVertices (vertices);
		mf.mesh.SetTriangles (triangles, 0);

		mc.sharedMesh = null;
		mc.sharedMesh = mf.mesh;
		//missing the normals***
		//Missing the uvs***
	}
}
