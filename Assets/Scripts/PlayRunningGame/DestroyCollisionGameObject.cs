using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DestroyCollisionGameObject : MonoBehaviour {

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	private void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.CompareTag( "DeadLine" ) ) {
			Destroy( this.gameObject );
		}
	}
}