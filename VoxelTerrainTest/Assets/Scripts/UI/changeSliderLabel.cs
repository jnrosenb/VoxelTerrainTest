using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class changeSliderLabel : MonoBehaviour 
{

	private Slider slider;
	private Text label;

	void Start()
	{
		slider = GetComponent<Slider> ();

		label = GetComponentsInChildren<Text> () [1] as Text;
	}

	// Use this for initialization
	public void OnSliderChange()
	{
		label.text =  (slider.value).ToString();
	}
}
