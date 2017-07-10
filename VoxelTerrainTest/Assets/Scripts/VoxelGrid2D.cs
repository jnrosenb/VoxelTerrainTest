using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class VoxelGrid2D : MonoBehaviour 
{
	private bool[] voxelsBits;
	private int width;
	private float voxelSize;
	private float gridSize;


	//Init method:
	public void gridInit(int res, float size)
	{
		gridSize = size;
		width = res;

		voxelsBits = new bool[width * width];

		//We will determine the voxel size by dividing the quad by the resolution.
		voxelSize = gridSize / width;

		for (int i = 0, y = 0; y < width; y++)
		{
			for (int x = 0; x < width; x++, i++)
			{
				createVoxel (i, x, y);
			}
		}
	}


	//Initializes a voxel in position x, y, at index i.
	private void createVoxel(int i, float x, float y)
	{
		x *= voxelSize;
		y *= voxelSize;

		GameObject quad = new GameObject ("Quad_" + i);
		quad.transform.SetParent (this.transform);
		MeshFilter mf = quad.AddComponent<MeshFilter> () as MeshFilter;
		MeshRenderer mr = quad.AddComponent<MeshRenderer> () as MeshRenderer;
		List<Vector3> vertices = new List<Vector3> () {
			new Vector3 (0, 0, 0), 
			new Vector3 (0, voxelSize, 0), 
			new Vector3 (voxelSize, voxelSize, 0), 
			new Vector3 (voxelSize, 0, 0)
		};
		mf.mesh.SetVertices (vertices);
		mf.mesh.SetTriangles (new int[]{ 0, 1, 2, 0, 2, 3}, 0);
		mr.material = new Material(Shader.Find("Diffuse"));
		mr.material.color = Color.white;

		quad.transform.localPosition = new Vector3 (x - gridSize/2f, y - gridSize/2f, 0f);
		//quad.transform.localScale *= 0.1f;

	}
}
