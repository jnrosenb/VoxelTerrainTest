using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour 
{
	public float jumpStrength = 3f;
	public float movementSpeed = 5f;
	public float rotateSpeed = 3f;

	public float maxHorizontalSpeed = 5f;
	public float maxVerticalSpeed = 5f;

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
					rgbdy.AddForce (new Vector3(transform.forward.x * movementSpeed, 0f, transform.forward.z * movementSpeed));
			}
			if (Input.GetKey (KeyCode.S))
			{
					rgbdy.AddForce (new Vector3(-transform.forward.x * movementSpeed, 0f, -transform.forward.z * movementSpeed));
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


			if (Input.GetKey (KeyCode.Mouse0))
			{
				if (rgbdy.velocity.y < maxVerticalSpeed)
					rgbdy.AddForce (Vector3.up * jumpStrength);
			}
			if (Input.GetKeyUp (KeyCode.Mouse0))
			{
				rgbdy.velocity = new Vector3(rgbdy.velocity.x, 0f, rgbdy.velocity.z);
			}
		}
		else
		{
			rgbdy.velocity = Vector3.zero;
		}
	}
}
