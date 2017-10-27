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
		transform.position = new Vector3(0,0,0); 
		transform.localScale = new Vector3 (_modelScale, _modelScale, _modelScale); 
	}

	public void SliderControl (float _slideScale)
	{
		_modelScale = _slideScale; 
	}
}
