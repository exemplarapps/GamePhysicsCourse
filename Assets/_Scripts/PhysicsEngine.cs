using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsEngine : MonoBehaviour {
	public float mass;
	public Vector3 velocityVector; //Average velocity this fixed update
	public Vector3 netForceVector;
	public bool showTrails = true;
	
	private List<Vector3> forceVectorList = new List<Vector3>();
	private LineRenderer lineRenderer;
	private int numberOfForces;
	private Vector3 deltaS;


	// Use this for initialization
	void Start () {
		deltaS = Vector3.zero;
		PrepareLines ();
	}


	void Update () {
		//show trails if its enabled
		if (showTrails) {
			ShowTrails ();
			return;
		} 

		lineRenderer.enabled = false;

	}

	void FixedUpdate () {
		UpdatePosition ();

	}

	void UpdatePosition(){
		//sum the forces and clear the list
		netForceVector = Vector3.zero;
		forceVectorList.ForEach (
			v => {
				netForceVector += v;
			}

		);
		forceVectorList.Clear ();

		//update the position
		velocityVector += (netForceVector / mass) * Time.deltaTime;
		deltaS += velocityVector * Time.deltaTime;
		transform.position = deltaS;
	}



	void PrepareLines () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.SetColors(Color.yellow, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.useWorldSpace = false;
	}


	void ShowTrails(){
		lineRenderer.enabled = true;
		numberOfForces = forceVectorList.Count;
		lineRenderer.SetVertexCount(numberOfForces * 2);
		int i = 0;
		foreach (Vector3 forceVector in forceVectorList) {
			lineRenderer.SetPosition(i, Vector3.zero);
			lineRenderer.SetPosition(i+1, -forceVector);
			i = i + 2;
		}
	}

	public void AddForce(Vector3 forceVector){
		forceVectorList.Add (forceVector);
	}
}
