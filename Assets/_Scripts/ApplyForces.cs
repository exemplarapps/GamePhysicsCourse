using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsEngine))]
public class ApplyForces : MonoBehaviour {

	public Vector3 forceVector = Vector3.zero;
	private PhysicsEngine physE;

	// Use this for initialization
	void Start () {
		physE = gameObject.GetComponent<PhysicsEngine> ();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		physE.AddForce (forceVector);
	}
}
