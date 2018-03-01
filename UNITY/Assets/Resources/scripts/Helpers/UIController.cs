using UnityEngine;
using UnityEngine.UI;


public class UIController : MonoBehaviour
{
	public GameObject _modelDisplay;
	public GameObject _volpeModel;
	public GameObject _AndorraModel;

	public Text _calibrationButtonTxt;
	public Text _contextButtonTxt;

	private bool _bool;

	void Start ()
	{
		_modelDisplay.SetActive (true); 
		_volpeModel.SetActive (false); 
		_AndorraModel.SetActive (false); 
	}

	// On Off Calibration
	public void CalibrationButton ()
	{
		_modelDisplay.SetActive (_bool); 

		if (_bool) {
			_calibrationButtonTxt.text = "Start Calibration";
		} else {
			_calibrationButtonTxt.text = "End Calibration"; 
		}
		_bool = !_bool;
	}

	// Context models
	public void ContextModels ()
	{
		_volpeModel.SetActive (_bool); 
		_AndorraModel.SetActive (!_bool); 

		if (_bool) {
			_contextButtonTxt.text = "Andorra";
		} else {
			_contextButtonTxt.text = "Kendall Sq."; 
		}
		_bool = !_bool;
	}

}