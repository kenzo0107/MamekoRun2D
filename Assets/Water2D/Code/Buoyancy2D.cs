/// <summary>
/// Bouyancy2D. Author: Cesar Rios 2013
/// This will use the top of a box collider as water level
/// </summary>

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Buoyancy2D : MonoBehaviour {
	
	/// <summary>
	/// The drag coeficent of the water. Will slowdown translation underwater
	/// </summary>
	public float		dragCoefficent = 0.4f;
	
	/// <summary>
	/// The angular drag coeficent of the water. Will slow down rotations underwater
	/// </summary>
	public float		angularDragCoefficent = 0.4f;
	
	/// <summary> 
	/// The bouyancy factor. For tweaking floating values
	/// </summary>
	public float 		buoyancyFactor = 0.001f;
	
	//Cache vars
	private BoxCollider			myBoxCollider;
	private float				gravity;
	private Transform			myTransform;
	
	//Affected objects
	private List<Rigidbody>				rigidbodiesUnderWater = new List<Rigidbody>();
	private List<Transform>				transformsUnderWater = new List<Transform>();
	private List<FloatingObject>		floatingObjectUnderWater = new List<FloatingObject>();
	
	
	// Use this for initialization
	void Start () {
		
		//Cache vars
		myBoxCollider 		= collider as BoxCollider;
		gravity 			= Mathf.Abs(Physics.gravity.y);
		myTransform 		= transform;
	}
	
	
	
	
	// Update is called once per frame
	void FixedUpdate () {
		
		if (myBoxCollider == null)  // WARNING RETURN!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			return;

		float waterLevel = myTransform.TransformPoint(myBoxCollider.extents).y;
		
		//Apply bouyancy forces
		for (int i= 0; i< rigidbodiesUnderWater.Count; i++)
		{
			//Bouyancy
			float objectArea 		=	floatingObjectUnderWater[i].floatingArea.x*floatingObjectUnderWater[i].floatingArea.y;
			float distanceSink		=	waterLevel-(transformsUnderWater[i].position.y-floatingObjectUnderWater[i].floatingArea.y*0.5f);
			float bouyancyModifier	=	distanceSink/floatingObjectUnderWater[i].floatingArea.y;  //Percentage of object sink
			bouyancyModifier 		= 	Mathf.Clamp01(bouyancyModifier);
				
			
			Vector3 bouyancyContributtion = Vector3.up*bouyancyModifier*buoyancyFactor*objectArea/rigidbodiesUnderWater[i].mass*gravity;
			
			
			//Water drag
			Vector3 waterDrag = -rigidbodiesUnderWater[i].velocity*dragCoefficent*rigidbodiesUnderWater[i].mass;
			
			
			//Force sum
			Vector3 forceToApply = bouyancyContributtion + waterDrag;
			
			Debug.DrawRay(transformsUnderWater[i].position, waterDrag, Color.red);
			Debug.DrawRay(transformsUnderWater[i].position, bouyancyContributtion, Color.yellow);
			
			//Apply force
			rigidbodiesUnderWater[i].AddForce(forceToApply, ForceMode.Force);
			
			//Angular drag
			Vector3 angularDrag = -rigidbodiesUnderWater[i].angularVelocity.normalized*rigidbodiesUnderWater[i].angularVelocity.sqrMagnitude*angularDragCoefficent;
			rigidbodiesUnderWater[i].AddTorque(angularDrag, ForceMode.Force);
		}
	}
	
	
	void OnTriggerEnter(Collider _collider)
	{
		
		FloatingObject tempFloatingObject = _collider.GetComponent<FloatingObject>();
		
		if (tempFloatingObject != null)
		{
			// Cache componentes 
			rigidbodiesUnderWater.Add(_collider.rigidbody);
			transformsUnderWater.Add(_collider.transform);
			floatingObjectUnderWater.Add(tempFloatingObject);
		}
		
	}
	
	void OnTriggerExit(Collider _collider)
	{
		FloatingObject tempFloatingObject = _collider.GetComponent<FloatingObject>();
		
		if (tempFloatingObject != null)
		{
			//Free components
			rigidbodiesUnderWater.Remove(_collider.rigidbody);
			transformsUnderWater.Remove(_collider.transform);
			floatingObjectUnderWater.Remove(tempFloatingObject);
		}	
	}
	
}
