using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class VoxelMap3D : MonoBehaviour 
{
	
	//This s a template for a gameobject with chunkManager script:
	public GameObject chunkTemplate; 
	public GameObject player;
	private Vector2 playerPrevPos;

	//Useful private variables:
	private float radius;// = 4f;
	private Vector3 chunkVoxelRes;
	private float voxelSize;
	private Vector3 chunkSize;
	private bool createNewChunksInGame;// = false;
	private bool halfInterpolation;// = false;
	//[Range(-1.0f, 1.0f)]
	private float isovalue;// = 0f;

	//Hashing table that will contain the chunks:
	private Dictionary<Vector2, VoxelChunk> chunkDic;
	private List<Action> methodsToCallOnMain;



	//Replacement for method start():
	public void StartGeneration (List<System.Object> args) 
	{
		chunkDic = new Dictionary<Vector2, VoxelChunk> ();
		methodsToCallOnMain = new List<Action> ();
		playerPrevPos = Vector2.zero;

		//[0]: Isovalues, [1]: ChunkDimensions, [2]: Voxelwidth, [3]: radius, [4]: halfInterpolation, [5]: chunkLoading.
		isovalue = (float)args[0];
		chunkVoxelRes = (Vector3)args[1];
		voxelSize = (float)args[3];
		radius = (float)args[2];
		halfInterpolation = (bool)args[4];
		createNewChunksInGame = (bool)args[5];

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
					chunkManager manager = chunk.GetComponent<chunkManager> ();
					chunk.transform.SetParent (transform);
					chunk.transform.localPosition = new Vector3 ((i - radius) * chunkSize.x, 0f,  (j - radius) * chunkSize.z);
					chunkDic.Add (chunkDictCoords, manager.chunk);

					//[0]: starting pos offset, [1]: chunkSize, [2]: VoxelRes, [3]: Isovalue, [4]seeBounds:
					manager.StartChunkManager (new VoxelChunk (new List<System.Object> ()
					{
						new Vector2(i - radius, j - radius), 
						chunkSize, 
						chunkVoxelRes, 
						isovalue, 
						halfInterpolation, 
						voxelSize
					}));

					manager.SetUnityMesh ();
				}
			}
		}
	}



	//Update will be called every frame:
	void Update()
	{
		if (createNewChunksInGame)
		{
			Vector2 playerGridPos = new Vector2 ((int)((player.transform.position.x)/(chunkSize.x)), (int)((player.transform.position.z)/(chunkSize.z)));
			//Debug.Log ("(" + (int)player.transform.position.x + "," +  (int)player.transform.position.z + ") / " + (int)chunkSize.x + ": ---> "  + playerGridPos.ToString());

			if (playerGridPos != playerPrevPos)
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
								threadManager (chunkDictCoords, playerGridPos, i, j);
						}
					}
				}

				while(methodsToCallOnMain.Count > 0)
				{
					Action func = methodsToCallOnMain [0];
					func ();
					methodsToCallOnMain.RemoveAt (0);
				}
			}

			playerPrevPos = playerGridPos;
		}
	}



	//Coroutine for managing the time slicing better:
	void threadManager(Vector2 chunkDictCoords, Vector2 playerGridPos, int i, int j)
	{
		//Part1: Instantiate what needs to be instantiated:
		GameObject chunk = Instantiate (chunkTemplate);
		chunkManager manager = chunk.GetComponent<chunkManager> ();
		chunk.transform.SetParent (transform);
		chunk.transform.localPosition = new Vector3 ((playerGridPos.x + i - radius) * chunkSize.x, 0f, (playerGridPos.y + j - radius) * chunkSize.z);
		chunkDic.Add (chunkDictCoords, manager.chunk);

		// Part 2, take the heavy computation section and throw it into a thread:
		Thread t1 = new Thread (() => threadJob (manager, chunkDictCoords));
		t1.Start ();
	}



	//Coroutine/Threading testing:
	void threadJob(chunkManager manager, Vector2 chunkDictCoords)
	{
		manager.StartChunkManager (new VoxelChunk (new List<System.Object> () {
			chunkDictCoords,
			chunkSize,
			chunkVoxelRes,
			isovalue,
			halfInterpolation,
			voxelSize
		}));
			
		Action func = () => 
		{ 
			manager.SetUnityMesh (); 
		};
		methodsToCallOnMain.Add (func);
	}

}//End-of-voxelMap-class-
