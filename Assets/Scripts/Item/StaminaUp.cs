using UnityEngine;
using System.Collections;

public class StaminaUp : MonoBehaviour {
	
	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.CompareTag( "Player" ) ) {
			// コイン取得音再生.
			Audio.AudioManager.Instance.PlaySE( "se_cute_rising_sequence_01" );
			// Destroy.
			Destroy ( this.gameObject );
		}
	}
}
