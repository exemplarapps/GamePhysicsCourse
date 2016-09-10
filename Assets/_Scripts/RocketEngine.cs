using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhysicsEngine))]
public class RocketEngine : MonoBehaviour {
	public float fuelMass;							// [k]
	public float maxThurst; 						// kN [kg m s^-1]

	[Range(0,1f)]
	public float thurstPercent;						// [none]
	public Vector3 thurstUnitVector = Vector3.zero; // [none]
	private PhysicsEngine physE;
	private float currentThrust; 					// N

	// Use this for initialization
	void Start () {
		physE = gameObject.GetComponent<PhysicsEngine> ();
		physE.mass += fuelMass;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (fuelMass > FuelThisUpdate ()) {
			fuelMass -= FuelThisUpdate ();
			physE.mass -= FuelThisUpdate ();
			//physE.AddForce (thurstUnitVector);
			ExertForce ();
		} else {
			Debug.LogWarning ("Out of rocket Fuel");
		}
	}

	void ExertForce(){
		currentThrust = thurstPercent * maxThurst * 1000f; //kN
		Vector3 thrustVector = thurstUnitVector.normalized * currentThrust; // N
		if (thrustVector != Vector3.zero) {
			physE.AddForce (thrustVector);
		}
	}

	float FuelThisUpdate(){
		float exhaustMassFlowRate;			// [kg s^-1]
		float effectiveExhaustVelocity = 4462f;		//  [m s^-1]

		exhaustMassFlowRate = currentThrust / effectiveExhaustVelocity;


		return exhaustMassFlowRate * Time.deltaTime;
	}
}
