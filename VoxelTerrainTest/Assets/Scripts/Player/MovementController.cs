using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour 
{
	public float jumpStrength = 3f;
	public float movementSpeed = 5f;
	public float rotateSpeed = 3f;

	public Vector3 gridPosition;

	private Rigidbody rgbdy;


	// Use this for initialization
	void Start () 
	{
		rgbdy = GetComponent<Rigidbody> ();		

		gridPosition = Vector3.zero;
	}

	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (!globalVars.paused)
		{
			if (Input.GetKey (KeyCode.W))
			{
				rgbdy.AddForce (transform.forward * movementSpeed);
			}
			if (Input.GetKey (KeyCode.S))
			{
				rgbdy.AddForce (-transform.forward * movementSpeed);
			}
			if (Input.GetKey (KeyCode.D))
			{
				rgbdy.AddForce (transform.right * movementSpeed);
			}
			if (Input.GetKey (KeyCode.A))
			{
				rgbdy.AddForce (-transform.right * movementSpeed);
			}

			GetComponentInChildren<Transform> ().Rotate (0f, Input.GetAxis ("Mouse X") * Time.deltaTime * 200f, 0f, Space.World);
			GetComponentInChildren<Transform> ().Rotate (-Input.GetAxis ("Mouse Y") * Time.deltaTime * 200f, 0f, 0f, Space.Self);


			if (Input.GetKeyDown (KeyCode.Mouse0))
			{
				rgbdy.AddForce (Vector3.up * jumpStrength);
			}
		}
		else
		{
			rgbdy.velocity = Vector3.zero;
		}
	}
}
