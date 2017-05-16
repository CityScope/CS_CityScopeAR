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

	public void hideObjects ()
	{
		_bool = !_bool;
		print ("click"); 
		_modelToHide.SetActive (_bool); 
		_arLineToHide.SetActive (!_bool); 

		if (_bool) {
			_buttonText.text = "Turn On Measure";
		} else {
			_buttonText.text = "Measuring.."; 
		}
	}



}






