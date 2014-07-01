using UnityEngine;
using System.Collections;

public class FloatingObject : MonoBehaviour {
	
	public Vector3	floatingArea;
	
	void OnDrawGizmosSelected()
	{
		Gizmos.DrawWireCube(transform.position, floatingArea);	
		
	}
}
