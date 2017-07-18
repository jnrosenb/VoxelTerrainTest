using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VoxelMap3D : MonoBehaviour 
{
	//Template and player vars:
	public GameObject chunkTemplate;
	public GameObject player;
	private Vector2 playerPrevPos;

	//Useful public variables:
	public float radius = 4f;
	public Vector3 chunkVoxelRes;
	public float voxelSize;
	private Vector3 chunkSize;

	[Range(-1.0f, 1.0f)]
	public float isovalue = 0f;
	public bool halfInterpolation = false;

	//Hashing table that will contain the chunks:
	private Dictionary<Vector2, VoxelChunk> chunkDic;


	// Use this for initialization
	void Start () 
	{
		chunkDic = new Dictionary<Vector2, VoxelChunk> ();
		playerPrevPos = Vector2.zero;

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


	//Update will be called every frame:
	void Update()
	{
		Vector2 playerGridPos = new Vector2 ((int)((player.transform.position.x)/(chunkSize.x/2f)), (int)((player.transform.position.z)/(chunkSize.z/2f)));

		if (playerGridPos.x != playerPrevPos.x || playerGridPos.y != playerPrevPos.y)
		{
			//Circular like world, starting in the center outwards:
			float amount = (2 * radius + 1);
			for (int i = 0; i < amount; i++)
			{
				for (int j = 0; j < amount; j++)
				{
					Vector2 chunkDictCoords = new Vector2 (playerGridPos.x + i - radius, playerGridPos.y + j - radius);
					if (Vector2.Distance (chunkDictCoords, playerGridPos) <= radius)
					{
						if (!chunkDic.ContainsKey (chunkDictCoords))
						{
							GameObject chunk = Instantiate (chunkTemplate);

							//chunk.SendMessage ("postStart", new List<System.Object> () {chunkDictCoords, chunkSize, chunkVoxelRes, isovalue, halfInterpolation, voxelSize});
							chunk.GetComponent<VoxelChunk> ().postStart (new List<System.Object> () {
								chunkDictCoords,
								chunkSize,
								chunkVoxelRes,
								isovalue,
								halfInterpolation,
								voxelSize
							});

							chunk.transform.SetParent (transform);

							chunk.transform.localPosition = new Vector3 ((playerGridPos.x + i - radius) * chunkSize.x, 0f, (playerGridPos.y + j - radius) * chunkSize.z);
							chunkDic.Add (chunkDictCoords, chunk.GetComponent<VoxelChunk> ());
						}

					}
				}
			}
		}

		playerPrevPos = playerGridPos;
	}//End-of-Update-method--

}
