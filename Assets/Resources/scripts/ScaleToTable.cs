using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleToTable : MonoBehaviour
{
	/// <summary>
	/// cityIO holder GO
	/// </summary>
	public GameObject _cityIO;

	public void ScaleToLegoSize ()
	{
		// cityIO position and scale 
		_cityIO.transform.localScale = new Vector3 (0.0015f, 0.0015f, 0.0015f); 
	}
}
