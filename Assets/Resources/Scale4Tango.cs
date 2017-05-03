using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scale4Tango : MonoBehaviour
{

//	[Range (0.001f, 0.1f)]
	public float _modelScale;


	// Update is called once per frame
	void Update ()
	{
		transform.localScale = new Vector3 (_modelScale, _modelScale, _modelScale); 
		//transform.localPosition = new Vector3 (0,0,0); 

	}

	public void SliderControl (float _slideScale)
	{
		_modelScale = _slideScale; 
	}
}
