using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMap3D : MonoBehaviour 
{
	//Template
	public GameObject chunkTemplate;

	//Useful public variables:
	public float isovalue = 0f;
	public float radius = 4f;
	public float chunkSize = 100f;
	public int chunkVoxelRes = 30;

	private Dictionary<Vector2, VoxelChunk> chunkDic;


	// Use this for initialization
	void Start () 
	{
		chunkDic = new Dictionary<Vector2, VoxelChunk> ();

		//Circular like world, starting in the center outwards:
		float amount = (2 * radius + 1);
		for (int i = 0; i < amount; i++)
		{
			for (int j = 0; j < amount; j++)
			{
				Vector2 chunkDictCoords = new Vector2 (i - radius, j - radius);

				if (Vector2.Distance (chunkDictCoords, Vector2.zero) <= radius)
				{
					GameObject chunk = Instantiate (chunkTemplate);

					//Sends the following: first, the coords in the spherical 2d grid. 
					//then, the chunk's width. Finally, the voxelRes in each chunk:
					chunk.SendMessage ("postStart", new Vector4(i - radius, j - radius, chunkSize, chunkVoxelRes));

					chunk.transform.SetParent (transform);

					//Here we position the chunk relative to their coord in the chunk 2d grid:
					chunk.transform.localPosition = new Vector3 ((i - radius) * chunkSize, 0f,  (j - radius) * chunkSize);

					chunkDic.Add (chunkDictCoords, chunk.GetComponent<VoxelChunk> ());
				}
			}
		}
	}

}
