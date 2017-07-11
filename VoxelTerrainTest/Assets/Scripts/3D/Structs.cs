using UnityEngine;

/*
 * BASED ON PAUL BOURKE'S CODE
 * http://paulbourke.net/geometry/polygonise/
*/

//Each triangle contains: 3 vertices.
public struct Triangle
{
	public Vector3[] p;

	public Triangle(Vector3[] vertices)
	{
		p = vertices;
	}
};


//Each gridCell contains: An array of 8 vertices. An array of 8 values (the values at each vertex).
public struct GridCell
{
	public Vector3[] p;
	public float[] val;

	public GridCell(Vector3[] vertices, float[] values)
	{
		p = vertices;
		val = values;
	}
};
