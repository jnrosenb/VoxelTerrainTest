/*
 * BASED ON PAUL BOURKE'S CODE
 * http://paulbourke.net/geometry/polygonise/
*/


//This represents a vertex:
public struct XYZ
{
	public float x;
	public float y;
	public float z;

	public XYZ(float X, float Y, float Z)
	{
		x = X;
		y = Y;
		z = Z;
	}
};


//Each triangle contains: 3 vertices.
public struct Triangle
{
	public XYZ[] p;

	public Triangle(XYZ[] vertices)
	{
		p = vertices;
	}
};


//Each gridCell contains: An array of 8 vertices. An array of 8 values (the values at each vertex).
public struct GridCell
{
	public XYZ[] p;
	public float[] val;

	public GridCell(XYZ[] vertices, float[] values)
	{
		p = vertices;
		val = values;
	}
};
