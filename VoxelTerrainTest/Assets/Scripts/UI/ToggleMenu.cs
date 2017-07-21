using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleMenu : MonoBehaviour
{
	public void toggleMenu()
	{
		if (this.gameObject.activeInHierarchy)
		{
			this.gameObject.SetActive (false);
			globalVars.paused = false;
		}
		else
		{
			globalVars.paused = true;
			this.gameObject.SetActive (true);
		}
	}

	public void exitGame()
	{
		Application.Quit ();
	}
}
