//-----------------------------------------------------------------------
// <copyright file="PointToPointGUIController.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

// based on point to point scene in Tango SDK samples 


using System;
using System.Collections;
using System.Collections.Generic;
using Tango;
using UnityEngine;

/// <summary>
/// GUI controller to show distance data.
/// </summary>
public class Calibration : MonoBehaviour, ITangoDepth
{
	// Constant values for overlay.
	public const float UI_LABEL_START_X = 15.0f;
	public const float UI_LABEL_START_Y = 15.0f;
	public const float UI_LABEL_SIZE_X = 1920.0f;
	public const float UI_LABEL_SIZE_Y = 35.0f;

	/// <summary>
	/// size of lego cell in meters
	/// </summary>
	public int _cellSize = 22;
	/// <summary>
	/// how many cells are in a raw (usually 15/16)
	/// </summary>
	public int _cellsInRaw = 15;

	/// <summary>
	/// cityIO holder GO
	/// </summary>
	public GameObject _cityIO;

	/// <summary>
	/// The point cloud object in the scene.
	/// </summary>
	public TangoPointCloud m_pointCloud;

	/// <summary>
	/// The line renderer to draw a line between two points.
	/// </summary>
	public LineRenderer m_lineRenderer;

	/// <summary>
	/// The scene's Tango application.
	/// </summary>
	private TangoApplication m_tangoApplication;

	/// <summary>
	/// If set, then the depth camera is on and we are waiting for the next
	/// depth update.
	/// </summary>
	private bool m_waitingForDepth;

	/// <summary>
	/// The older of the two points to measure.
	/// </summary>
	private Vector3 m_startPoint = new Vector3 (0.0f, 0, 0);

	/// <summary>
	/// The newer of the two points to measure.
	/// </summary>
	private Vector3 m_endPoint = new Vector3 (1f, 0, 0);
	//avoid zreo scale at start !


	/// <summary>
	/// The text to display the distance.
	/// </summary>
	private string m_distanceText;

	private GameObject _sphereStart;
	private GameObject _sphereEnd;
	private float _sphereScale = 0.02f;
	public Material _spheresMaterial;
	public int _rotationOffset;
	public TextMesh _3dText;
	private TextMesh _first3dText;
	private TextMesh _second3dText;
	private Quaternion _lineRot;

	/// <summary>
	/// Start this instance.
	/// </summary>
	public void Start ()
	{
		m_tangoApplication = FindObjectOfType<TangoApplication> ();
		m_tangoApplication.Register (this);
	}

	public void Update ()
	{
		_RenderLine (); //go and print the line and scale/rot the model 

		if (Input.GetMouseButtonUp (0)) { // allow dragging 
			StartCoroutine (_WaitForDepth (Input.mousePosition));
		}
		if (Input.GetKey (KeyCode.Escape)) {
			// This is a fix for a lifecycle issue where calling
			// Application.Quit() here, and restarting the application
			// immediately results in a deadlocked app.
			AndroidHelper.AndroidQuit ();
		}
	}

	/// <summary>
	/// Wait for the next depth update, then find the nearest point in the point
	/// cloud.
	/// </summary>
	/// <param name="touchPosition">Touch position on the screen.</param>
	/// <returns>Coroutine IEnumerator.</returns>
	private IEnumerator _WaitForDepth (Vector2 touchPosition)
	{
		m_waitingForDepth = true;
		// Turn on the camera and wait for a single depth update
		m_tangoApplication.SetDepthCameraRate (
			TangoEnums.TangoDepthCameraRate.MAXIMUM);
		while (m_waitingForDepth) {
			yield return null;
		}
		m_tangoApplication.SetDepthCameraRate (
			TangoEnums.TangoDepthCameraRate.DISABLED);
		Camera cam = Camera.main;
		int pointIndex = m_pointCloud.FindClosestPoint (cam, touchPosition, 10);
		if (pointIndex > -1) {
			// Index is valid
			m_startPoint = m_endPoint;
			m_endPoint = m_pointCloud.m_points [pointIndex];
		}
	}


	private void _RenderLine ()
	{
		//removes old GO's 
		Destroy (_sphereStart); 
		Destroy (_sphereEnd);  
		Destroy (_first3dText, 2);


		m_lineRenderer.SetPosition (0, m_startPoint);
		m_lineRenderer.SetPosition (1, m_endPoint);
		Vector3 _delta = m_endPoint - m_startPoint;
		_lineRot = Quaternion.LookRotation (_delta);
		Vector3 _rotEular = _lineRot.eulerAngles;
		float _dist = Vector3.Distance (m_startPoint, m_endPoint);
		_dist = _dist / (_cellSize * _cellsInRaw); 

		// cityIO position and scale 
		_cityIO.transform.position = m_startPoint; //set base point
		_cityIO.transform.rotation = Quaternion.Euler (_rotEular.x, _rotEular.y + _rotationOffset, _rotEular.z); // move the rotation to align to easy two points 
		_cityIO.transform.localScale = new Vector3 (_dist, _dist, _dist); 


		//3d text 
		_first3dText = Instantiate (_3dText);
		_first3dText.transform.position = new Vector3 (m_startPoint.x, m_startPoint.y + 0.015f, m_startPoint.z); 
		//_first3dText.transform.LookAt(Camera.main.transform);
		_first3dText.transform.localScale = new Vector3 (_sphereScale, _sphereScale, _sphereScale); 
		_first3dText.transform.rotation = _cityIO.transform.rotation; 
		_first3dText.text = "First" + "\n" + "Point"; 
		_first3dText.transform.parent = transform; 

		//End, start Spheres 
		Color _tmpColor = _spheresMaterial.color; 
		_tmpColor.a = 0.25f; 
		_spheresMaterial.color = _tmpColor; 
		_tmpColor = new Color (1, 0, 0, 0.25f);

		_sphereStart = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		_sphereStart.transform.parent = transform; 
		_sphereStart.transform.position = m_startPoint;
		_sphereStart.transform.localScale = new Vector3 (_sphereScale, _sphereScale, _sphereScale);
		_sphereStart.GetComponent<Renderer> ().material = _spheresMaterial;

		_sphereEnd = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		_sphereEnd.transform.parent = transform; 
		_sphereEnd.transform.position = m_endPoint;
		_sphereEnd.transform.localScale = new Vector3 (_sphereScale, _sphereScale, _sphereScale); 
		_sphereEnd.GetComponent<Renderer> ().material = _spheresMaterial;


//		for (int i = 0; i < _cellsInRaw; i++) {
//			GameObject _gridPoint = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//			_gridPoint.transform.position = Vector3.Lerp 
//				(m_endPoint, m_startPoint, 
//				(i * (_delta.magnitude / _cellsInRaw)));
//
//			print (_delta.magnitude + "  " + i + "  " + (i * (_delta.magnitude / _cellsInRaw)));  
//
//			_gridPoint.transform.localScale = new Vector3 (_sphereScale, _sphereScale, _sphereScale); 
//			print (_delta.magnitude);  
//		}
	}



	public void OnTangoDepthAvailable (TangoUnityDepth tangoDepth)
	{
		m_waitingForDepth = false;
	}

	public void OnTangoServiceConnected ()
	{
		m_tangoApplication.SetDepthCameraRate (
			TangoEnums.TangoDepthCameraRate.DISABLED);
	}

	public void OnTangoServiceDisconnected ()
	{
	}

	public void OnDestroy ()
	{
		m_tangoApplication.Unregister (this);
	}
}
