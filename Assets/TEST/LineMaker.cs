using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GUI controller to show distance data.
/// </summary>
public class LineMaker : MonoBehaviour
{

	public static List<GameObject> _PointsList = new List<GameObject> ();
	public LineRenderer _lineRenderer;
	private Vector3 _startPoint;
	private Vector3 _endPoint;
	private Vector3 _newTarget;

	public void Start ()
	{
		for (int i = 0; i < 5; i++) {
			var _sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			_PointsList.Add (_sphere);
			_lineRenderer.SetVertexCount (_PointsList.Count);
			_sphere.transform.position = new Vector3 (Random.Range (-10.0f, 10.0f), 
				Random.Range (-10.0f, 10.0f),
				Random.Range (-10.0f, 10.0f)); 
		}
	}

	public void Update ()
	{
		
		for (int i = 0; i < _PointsList.Count; i++) {

			_newTarget = new Vector3 (Random.Range (-10.0f, 10.0f), 
				Random.Range (-10.0f, 10.0f),
				Random.Range (-10.0f, 10.0f)); 

			_PointsList [i].transform.position = 
				Vector3.Lerp (_PointsList [i].transform.position, _newTarget, Time.deltaTime ); 
			_lineRenderer.SetPosition (i, _PointsList [i].transform.position);

		}
	}
}
