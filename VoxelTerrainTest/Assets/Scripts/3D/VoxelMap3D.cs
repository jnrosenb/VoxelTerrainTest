using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VoxelMap3D : MonoBehaviour 
{
	//Template
	public GameObject chunkTemplate;

	//Useful public variables:
	public float radius = 4f;
	public Vector3 chunkVoxelRes;
	public float voxelSize;
	private Vector3 chunkSize;

	[Range(-1.0f, 1.0f)]
	public float isovalue = 0f;
	public bool halfInterpolation = false;

	private Dictionary<Vector2, VoxelChunk> chunkDic;


	// Use this for initialization
	void Start () 
	{
		chunkDic = new Dictionary<Vector2, VoxelChunk> ();

		chunkSize.x = chunkVoxelRes.x * voxelSize;
		chunkSize.y = chunkVoxelRes.y * voxelSize;
		chunkSize.z = chunkVoxelRes.z * voxelSize;

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
					//[0]: starting pos offset, [1]: chunkSize, [2]: VoxelRes, [3]: Isovalue, [4]seeBounds.
					chunk.SendMessage ("postStart", new List<System.Object>(){new Vector2(i - radius, j - radius), chunkSize, chunkVoxelRes, isovalue, halfInterpolation, voxelSize});
					chunk.transform.SetParent (transform);

					//Here we position the chunk relative to their coord in the chunk 2d grid:
					chunk.transform.localPosition = new Vector3 ((i - radius) * chunkSize.x, 0f,  (j - radius) * chunkSize.z);
					chunkDic.Add (chunkDictCoords, chunk.GetComponent<VoxelChunk> ());
				}
			}
		}
	}

}
