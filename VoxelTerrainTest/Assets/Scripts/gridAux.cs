using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridAux : MonoBehaviour 
{

	public VoxelGrid2D gridPrefab;

	// Use this for initialization
	void Start () 
	{
		VoxelGrid2D chunk = GameObject.Instantiate (gridPrefab, transform).GetComponent<VoxelGrid2D>() as VoxelGrid2D;
		chunk.gridInit (10, 2);
	}
}
