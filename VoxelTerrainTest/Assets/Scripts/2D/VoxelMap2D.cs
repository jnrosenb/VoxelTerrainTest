using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class VoxelMap2D : MonoBehaviour 
{
	//Size of the map in unity units:
	public float size;
	//Width-Height of the grid in each chunk:
	public int voxelRes;
	//Width-Height of the map in term of chunks:
	public int chunkRes;
	private int chunkAmount;
	float chunkSize;

	//The grid template for generating the chunks:
	public VoxelGrid2D gridPrefab;
	private VoxelGrid2D[] chunks;


	// Use this for initialization
	void Awake () 
	{
		this.transform.position = Vector3.zero;

		chunkSize = size / chunkRes;
		chunkAmount = chunkRes * chunkRes;
		chunks = new VoxelGrid2D[chunkAmount];

		for (int i = 0, y = 0; y < chunkRes; y++)
		{
			for (int x = 0; x < chunkRes; x++, i++)
			{
				createChunk (i, x, y);
			}
		}

		BoxCollider coll = gameObject.AddComponent<BoxCollider> ();
		coll.center = Vector3.zero;
		coll.size = new Vector3(this.size, this.size);
	}

	
	// Update is called once per frame
	void createChunk (int i, int x, int y) 
	{
		VoxelGrid2D chunk = GameObject.Instantiate (gridPrefab, transform).GetComponent<VoxelGrid2D>();
		chunk.gridInit (voxelRes, chunkSize);

		chunk.transform.localPosition = new Vector3(x * chunkSize - chunkSize/2f, y * chunkSize - chunkSize/2f, 0f);

		chunks [i] = chunk;
	}


	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Mouse0))
		{
			RaycastHit hit;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit))
			{
				if (hit.collider.gameObject == this.gameObject)
				{
					editVoxels (transform.InverseTransformPoint(hit.point));
				}
			}
		}
	}


	void editVoxels(Vector3 position)
	{
		int x = (int)((position.x + size/2f) / size);
		int y = (int)((position.y + size/2f) / size);

		Debug.Log("You are clicking voxel in coord (" + x + "," + y + ").");
	}
}
