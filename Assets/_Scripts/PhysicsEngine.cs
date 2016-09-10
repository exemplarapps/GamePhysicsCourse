using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhysicsEngine : MonoBehaviour {
	public float mass;				// kg
	public Vector3 velocityVector; 	// N 
	public Vector3 netForceVector;	// N
	public bool showTrails = true;
	
	private List<Vector3> forceVectorList = new List<Vector3>();
	private LineRenderer lineRenderer;
	private int numberOfForces;
	private Vector3 deltaS; //kN
	private PhysicsEngine[] physicsEngineArray;
	private const float bigG = 6.673e-11f;  			//[m^3 kg^-1 s^-1]
	// Use this for initialization
	void Start () {
		//deltaS = Vector3.zero;
		PrepareLines ();
		physicsEngineArray = GameObject.FindObjectsOfType<PhysicsEngine> ();
	}


	void Update () {
	}

	void FixedUpdate () {
		ShowTrails ();
		CalculateGravity ();
		UpdatePosition ();

	}

	void LateUpdate(){
		//ShowTrails ();
	}
	public void CalculateGravity(){
		foreach (PhysicsEngine peA in physicsEngineArray) {
			foreach(PhysicsEngine peB in physicsEngineArray){
				if(peA != peB && peA != this){
					Debug.Log("computing force exerrted on " + peA.name + " due to gravity of " + peB.name);
					Vector3 offset = peA.transform.position - peB.transform.position;
					float rSquared = Mathf.Pow (offset.magnitude, 2f);
					float gravityMagnitude = bigG * peA.mass * peB.mass / rSquared;
					Vector3 gravityFeltVector = gravityMagnitude * offset.normalized;
					peA.AddForce (-gravityFeltVector);
				}
			}
		
		}

	}
	public void AddForce(Vector3 forceVector){
		forceVectorList.Add (forceVector);
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
		Vector3 accelerationVector = netForceVector / mass;
		velocityVector += accelerationVector * Time.deltaTime;
		//deltaS += velocityVector * Time.deltaTime;
		transform.position += velocityVector * Time.deltaTime;//deltaS;
	}



	void PrepareLines () {
		lineRenderer = gameObject.AddComponent<LineRenderer>();
		lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
		lineRenderer.SetColors(Color.yellow, Color.yellow);
		lineRenderer.SetWidth(0.2F, 0.2F);
		lineRenderer.useWorldSpace = false;
	}


	void ShowTrails(){
		//show trails if its enabled
		if (showTrails) {
			lineRenderer.enabled = true;
			numberOfForces = forceVectorList.Count; 
			lineRenderer.SetVertexCount(numberOfForces * 2);
			int i = 0;
			//Debug.Log (gameObject.name + " forces = " + numberOfForces.ToString ());
			foreach (Vector3 forceVector in forceVectorList) {

				//Debug.Log (gameObject.name + " force = " + forceVector);
				lineRenderer.SetPosition(i, Vector3.zero);
				lineRenderer.SetPosition(i+1, -forceVector);
				i = i + 2;
			}
		} else {

			lineRenderer.enabled = false;
		}
	}

}
