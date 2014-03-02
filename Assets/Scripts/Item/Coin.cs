using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	#region public members.
	public int			Score		= 10;
	public AudioClip	CoinClips;					// Array of clips for when the player jumps.
	#endregion

	#region private members.
	private GameObject	_gameManager;
	#endregion

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake( ) {
		_gameManager	= GameObject.Find ( "_GameManager" ).gameObject;
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D( Collider2D coll ) {
		if ( coll.gameObject.CompareTag( "Player" ) ) {
			// コイン取得音再生.
			AudioSource.PlayClipAtPoint( CoinClips, transform.position );

			// 点数加算.
			_gameManager.SendMessage( "addScore", Score );

			// Destroy.
			Destroy ( this.gameObject );
		}
	}
}
