using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class singletonScript : MonoBehaviour 
{

	public static singletonScript current;

	// Use this for initialization
	void Awake () 
	{
		//If it does not exist, we create and assign:
		if (ReferenceEquals (current, null))
		{
			current = this;
			DontDestroyOnLoad (this);
		}
		else
		{
			GameObject.Destroy (this);
		}
	}
}
