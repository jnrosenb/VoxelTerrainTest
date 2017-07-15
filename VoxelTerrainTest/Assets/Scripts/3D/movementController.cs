using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementController : MonoBehaviour 
{
	public float jumpStrength = 3f;
	public float movementSpeed = 5f;
	public float rotateSpeed = 3f;

	private Rigidbody rgbdy;

	// Use this for initialization
	void Start () 
	{
		rgbdy = GetComponent<Rigidbody> ();		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey (KeyCode.W))
		{
			rgbdy.AddForce (new Vector3(GetComponentInChildren<Transform>().forward.x, 0f, GetComponentInChildren<Transform>().forward.z) * movementSpeed);
		}
		if (Input.GetKey (KeyCode.S))
		{
			rgbdy.AddForce (-new Vector3(GetComponentInChildren<Transform>().forward.x, 0f, GetComponentInChildren<Transform>().forward.z) * movementSpeed);
		}
		if (Input.GetKey (KeyCode.D))
		{
			GetComponentInChildren<Transform>().Rotate (new Vector3(0f, rotateSpeed * Time.deltaTime, 0f), Space.Self);
		}
		if (Input.GetKey (KeyCode.A))
		{
			GetComponentInChildren<Transform>().Rotate (new Vector3(0f, -rotateSpeed * Time.deltaTime, 0f), Space.Self);
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			rgbdy.AddForce (Vector3.up * jumpStrength);
		}
	}
}
