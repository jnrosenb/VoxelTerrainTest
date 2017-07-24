using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGen : MonoBehaviour 
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

		setQuad (0, 2, 3, 1);
		setQuad (4, 0, 1, 5);
		setQuad (6, 2, 0, 4);
		setQuad (7, 3, 2, 6);
		setQuad (5, 1, 3, 7);
		setQuad (4, 5, 7, 6);
	}


	// Update is called once per frame
	private void setMesh () 
	{
		Mesh mesh = mf.mesh;

		mesh.SetVertices (this.vertices);
		mesh.SetTriangles (this.triangles, 0);

		mc.sharedMesh = null;
		mc.sharedMesh = mesh;

		mr.material = new Material (Shader.Find("Diffuse"));
		mr.material.color = Color.white;
	}


	//Given 4 vertices, it creates a quad in which the 2 triangles normals are aligned:
	private void setQuad(int a, int b, int c, int d)
	{
		triangles.Add (a);
		triangles.Add (b);
		triangles.Add (d);
		triangles.Add (b);
		triangles.Add (c);
		triangles.Add (d);
	}

}
