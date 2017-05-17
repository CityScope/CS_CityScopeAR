using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
	public GameObject _modelToHide;
	public GameObject _arLineToHide;
	public Text _buttonText;
	private bool _bool;

	void Start()

	{
		_modelToHide.SetActive (false); 
		_arLineToHide.SetActive (true); 
	}

	public void hideObjects ()
	{
		_bool = !_bool;
		_modelToHide.SetActive (_bool); 
		_arLineToHide.SetActive (!_bool); 

		if (_bool) {
			_buttonText.text = "Start Calibration";
		} else {
			_buttonText.text = "End Calibration"; 
		}
	}
}