using UnityEngine;
using System.Collections;

using Audio;

public class StaminaUp : MonoBehaviour {
	
	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.CompareTag( "Player" ) ) {
			// コイン取得音再生.
			AudioManager.Instance.PlaySE( AudioConfig.SePlayerStaminaUp );
			// Destroy.
			Destroy ( this.gameObject );
		}
	}
}
