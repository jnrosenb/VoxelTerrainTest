using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelGenerator : MonoBehaviour 
{
	private MeshFilter mf;
	private MeshRenderer mr;
	private MeshCollider mc;

	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();


	// Use this for initialization
	void Start () 
	{
		mf = this.GetComponent<MeshFilter> () as MeshFilter;
		mr = this.GetComponent<MeshRenderer> () as MeshRenderer;
		mc = this.GetComponent<MeshCollider> () as MeshCollider;

		setVertices (Vector3.zero);
		setMesh ();
	}


	// Update is called once per frame
	private void setVertices (Vector3 center, float halfDist = 1f) 
	{

		for (int y = 0; y < 2; y++)
		{
			for (int z = 0; z < 2; z++)
			{
				for (int x = 0; x < 2; x++)
				{
					vertices.Add(new Vector3(center.x + halfDist * Mathf.Pow(-1f, x), center.y + halfDist * Mathf.Pow(-1f, y), center.z + halfDist * Mathf.Pow(-1f, z)));
				}
			}
		}

		triangles.AddRange (new List<int>(){0,1,2,1,3,2,4,5,6,5,7,6,0,1,4,4,5,1,2,3,7,6,3,2,3,7,5,5,3,1,4,6,2,3,4,0});
	}


	// Update is called once per frame
	private void setMesh () 
	{
		Mesh mesh = mf.mesh;

		mesh.SetVertices (this.vertices);
		mesh.SetTriangles (this.triangles, 0);
		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();

		mc.sharedMesh = null;
		mc.sharedMesh = mesh;
	}
}
