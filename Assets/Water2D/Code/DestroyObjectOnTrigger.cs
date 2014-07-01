using UnityEngine;
using System.Collections;

public class DestroyObjectOnTrigger : MonoBehaviour {

	
	void OnTriggerEnter(Collider _collider)
	{
		Destroy(_collider.gameObject);	
	}
	
}
