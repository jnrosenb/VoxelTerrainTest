using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class VoxelChunk : MonoBehaviour 
{
	//Size of the cube that will be formed in unity units:
	public float size = 5f;
	//number of subdivitions per axis (determines number of cells)
	public int resolution = 5;
	private float voxelWidth;

	private MeshRenderer mr;
	private MeshFilter mf;
	private List<Vector3> vertices;
	private List<Color32> color;
	private List<int> triangles;


	// Use this for initialization
	void Start () 
	{
		this.mf = GetComponent<MeshFilter> ();
		this.mr = GetComponent<MeshRenderer> ();

		vertices = new List<Vector3>();
		color = new List<Color32>();
		triangles = new List<int>();

		voxelWidth = size / resolution;

		initMesh ();
	}


	private void initMesh()
	{
		setVertices ();
		mf.mesh.SetVertices (vertices);
		mf.mesh.SetColors (color);
	}


	//Set the vertices of the chunk. Starting always in origin:
	private void setVertices()
	{
		for (int i = 0, y = 0; y < resolution; y++)
		{
			for (int z = 0; z < resolution; z++)
			{
				for (int x = 0; x < resolution; x++, i++)
				{
					Vector3 vertex = new Vector3 (0f + x * voxelWidth, 0f + y * voxelWidth, 0f + z * voxelWidth);
					Color32 vertexColor = new Color32 ((byte)Random.Range(0, 255), 0, 0, 0);

					vertices.Add (vertex);
					color.Add (vertexColor);
				}
			}
		}
	}

}//EndOfClass: VoxelChunk-----//
