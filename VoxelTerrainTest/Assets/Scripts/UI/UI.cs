using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UI : MonoBehaviour 
{
	public GameObject voxelmapTemplate;
	public GameObject player;
	private VoxelMap3D current;

	float isovalue;
	Vector3 chunkDimensions;
	float voxelWidth;
	float radius;
	bool halfInterpolation;
	bool chunkLoading;

	public GameObject isoSlider;
	public GameObject xsizeBox;
	public GameObject ysizeBox;
	public GameObject zsizeBox;
	public GameObject radiusBox;
	public GameObject voxelWidthBox;
	public GameObject chunksToggle;
	public GameObject halfInterToggle;



	//Start method:
	void Start()
	{
		player = GameObject.Instantiate (player);

		//For now I'll place this here:
		player.transform.position = Vector3.zero;
	}



	//Method that generates the terrain based on the args on the UI:
	public void resetTerrain ()
	{
		isovalue = isoSlider.GetComponent<Slider>().value;

		float x, y, z;
		float.TryParse (xsizeBox.GetComponent<InputField> ().text, out x);
		float.TryParse (ysizeBox.GetComponent<InputField> ().text, out y);
		float.TryParse (zsizeBox.GetComponent<InputField> ().text, out z);
		chunkDimensions = new Vector3 (x, y, z);

		float.TryParse(voxelWidthBox.GetComponent<InputField>().text, out voxelWidth);
		float.TryParse(radiusBox.GetComponent<InputField>().text, out radius);
		halfInterpolation = halfInterToggle.GetComponent<Toggle>().isOn;
		chunkLoading = chunksToggle.GetComponent<Toggle>().isOn;

		List<System.Object> args = new List<System.Object> (){isovalue, chunkDimensions, voxelWidth, radius, halfInterpolation, chunkLoading};

		if (current != null)
			Destroy (current.gameObject);

		current = GameObject.Instantiate (voxelmapTemplate).GetComponent<VoxelMap3D>();
		current.name = "VoxelMap";
		player.name = "Player";
		current.player = player;

		current.StartGeneration (args);
	}

}
