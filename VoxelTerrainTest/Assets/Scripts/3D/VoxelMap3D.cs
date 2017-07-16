using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMap3D : MonoBehaviour 
{
	//Template
	public GameObject chunkTemplate;

	//Map width and height in term of chunks:
	public int zres = 5;
	public int xres = 5;

	//Temporary size for each chunk:
	public float chunkSize = 50f;
	private float xSize;
	private float zSize;

	//private Dictionary<Vector2, VoxelChunk> chunkDic;


	// Use this for initialization
	void Start () 
	{
		//chunkDic = new Dictionary<Vector2, VoxelChunk> ();

		xSize = xres * chunkSize;
		zSize = zres * chunkSize;

		for (int i = 0; i < zres; i++)
		{
			for (int j = 0; j < xres; j++)
			{
				GameObject chunk = Instantiate (chunkTemplate);
				chunk.SendMessage ("postStart", new Vector2(i, j));

				chunk.GetComponent<VoxelChunk> ().chunkSize = chunkSize; //This doesn't work since it might be called after awake-
				chunk.transform.SetParent (transform);

				chunk.transform.localPosition = new Vector3 (i * chunkSize, 0f,  j * chunkSize);
			}
		}

		transform.Translate (new Vector3(-xSize/2f, 0f, -zSize/2f));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
