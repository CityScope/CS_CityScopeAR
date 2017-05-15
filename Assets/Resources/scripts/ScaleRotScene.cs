using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleRotScene : MonoBehaviour
{

	public GameObject _firstGO;
	public GameObject _secondGO;
	private Renderer _rend;

	// Use this for initialization
	void Start ()
	{
		transform.position = new Vector3 (0, 0, 0); 
	}

	// Update is called once per frame
	void Update ()
	{
		transform.position = _firstGO.transform.position; //set base point

		float _dist = Vector3.Distance (_firstGO.transform.position, _secondGO.transform.position);
		Vector3 _delta = _secondGO.transform.position - _firstGO.transform.position;
		Quaternion _rot = Quaternion.LookRotation (_delta);

		print (" distance:  " + _dist + "  rot:  " + _rot.eulerAngles.x
		+ " Y>> " + _rot.eulerAngles.y + " X>> " + _rot.eulerAngles.z + "\n"); 

		transform.localScale = new Vector3 (_dist / 250, _dist / 250, _dist / 250); 
		transform.rotation = _rot; 

		_secondGO.transform.localScale = new Vector3 (_dist/5,_dist/5,_dist/5); 
		_firstGO.transform.localScale = new Vector3 (_dist/5,_dist/5,_dist/5); 


	}
}
