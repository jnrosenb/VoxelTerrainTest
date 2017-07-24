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



	//Replacement for method start():
	public void StartGeneration (List<System.Object> args) 
	{
		chunkDic = new Dictionary<Vector2, VoxelChunk> ();
		playerPrevPos = Vector2.zero;

		//[0]: Isovalues, [1]: ChunkDimensions, [2]: Voxelwidth, [3]: radius, [4]: halfInterpolation, [5]: chunkLoading.
		isovalue = (float)args[0];
		chunkVoxelRes = (Vector3)args[1];
		voxelSize = (float)args[2];
		radius = (float)args[3];
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

					//[0]: starting pos offset, [1]: chunkSize, [2]: VoxelRes, [3]: Isovalue, [4]seeBounds:
					manager.StartChunkManager (new VoxelChunk (new List<System.Object> (){
						new Vector2(i - radius, j - radius), 
						chunkSize, 
						chunkVoxelRes, 
						isovalue, 
						halfInterpolation, 
						voxelSize
					}));
					manager.SetUnityMesh ();

					chunk.transform.SetParent (transform);

					//Here we position the chunk relative to their coord in the chunk 2d grid:
					chunk.transform.localPosition = new Vector3 ((i - radius) * chunkSize.x, 0f,  (j - radius) * chunkSize.z);

					chunkDic.Add (chunkDictCoords, manager.chunk);
				}
			}
		}
	}



	//Update will be called every frame:
	void Update()
	{
		if (createNewChunksInGame)
		{
			Vector2 playerGridPos = new Vector2 ((int)((player.transform.position.x)/(chunkSize.x/2f)), (int)((player.transform.position.z)/(chunkSize.z/2f)));

			if (playerGridPos.x != playerPrevPos.x || playerGridPos.y != playerPrevPos.y)
			{

				//EXPERIMENT--------------------------------------------------------------
				int maxNumberOfThreads = radius > 0 ? (int)(radius * 3 - (radius - 1)) : 0;
				List<Thread> threads = new List<Thread> ();
				List<List<System.Object>> nestedList = new List<List<object>> ();
				//EXPERIMENT--------------------------------------------------------------

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
								threadManager(chunkDictCoords, playerGridPos, i, j, ref threads, ref nestedList);

								/*--ORIGINAL-CODE-----------------------------------------------------------------------------------------------
								GameObject chunk = Instantiate (chunkTemplate);
								chunkManager manager = chunk.GetComponent<chunkManager> ();

								Thread threadyJenny = new Thread (() => cr1 (manager, chunkDictCoords));
								threadyJenny.Start ();
								threadyJenny.Join ();
								//manager.chunk = new VoxelChunk (new List<System.Object> () {
								//	chunkDictCoords,
								//	chunkSize,
								//	chunkVoxelRes,
								//	isovalue,
								//	halfInterpolation,
								//	voxelSize
								//});
								manager.SetUnityMesh ();

								chunk.transform.SetParent (transform);

								chunk.transform.localPosition = new Vector3 ((playerGridPos.x + i - radius) * chunkSize.x, 0f, (playerGridPos.y + j - radius) * chunkSize.z);

								chunkDic.Add (chunkDictCoords, manager.chunk);
								//--ORIGINAL-CODE------------------------------------------------------------------------------------------------*/
							}
						}
					}
				}

				//EXPERIMENT--------------------------------------------------------------
				foreach(var thread in threads)
				{
					thread.Join ();
				}
				//EXPERIMENT--------------------------------------------------------------

				//EXPERIMENT----------------------------------------------------------------------------------------------------------------------------
				foreach (List<System.Object> list in nestedList)
				{
					chunkManager manager = list [0] as chunkManager;
					GameObject chunk = list [1] as GameObject;
					Vector2 chunkDictCoords = (Vector2)list [2];
					int i = (int)list [3];
					int j = (int)list [4];

					manager.SetUnityMesh ();
					chunk.transform.SetParent (transform);
					chunk.transform.localPosition = new Vector3 ((playerGridPos.x + i - radius) * chunkSize.x, 0f, (playerGridPos.y + j - radius) * chunkSize.z);
					chunkDic.Add (chunkDictCoords, manager.chunk);
				}
				//EXPERIMENT----------------------------------------------------------------------------------------------------------------------------

			}

			playerPrevPos = playerGridPos;
		}
	}//End-of-Update-method--



	//Coroutine for managing the time slicing better:
	void threadManager(Vector2 chunkDictCoords, Vector2 playerGridPos, int i, int j, ref List<Thread> threads, ref List<List<System.Object>> list)
	{
		// Part1: Instantiate what needs to be instantiated:
		GameObject chunk = Instantiate (chunkTemplate);
		chunkManager manager = chunk.GetComponent<chunkManager> ();


		//EXPERIMENT TO ELIMINATE BOTTLENECK:
		list.Add(new List<object>()
		{
			manager,
			chunk,
			chunkDictCoords,
			i, j
		});
		//EXPERIMENT TO ELIMINATE BOTTLENECK:

		// Part 2, take the heavy computation section and throw it into a thread:
		Thread t1 = new Thread (() => threadJob (manager, chunkDictCoords));
		threads.Add (t1);																//EXPERIMENT TO ELIMINATE BOTTLENECK:
		t1.Start ();

		//// Part 3 (bottleneck): Build into unity what has been done in the thread:
		//manager.SetUnityMesh ();
		//chunk.transform.SetParent (transform);
		//chunk.transform.localPosition = new Vector3 ((playerGridPos.x + i - radius) * chunkSize.x, 0f, (playerGridPos.y + j - radius) * chunkSize.z);
		//chunkDic.Add (chunkDictCoords, manager.chunk);
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
	}



	/*Use this for initialization
	void Start() 
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
	}//End of original start method*/

}//End-of-voxelMap-class-
