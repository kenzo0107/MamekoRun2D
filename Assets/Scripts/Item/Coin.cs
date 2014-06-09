using UnityEngine;
using System.Collections;

public class Coin : MonoBehaviour {

	#region public members.
	public int			Score		= 10;
	public AudioClip	CoinClips;					// Array of clips for when the player jumps.
	#endregion

	#region private members.
	private GameObject	gameManager;
	#endregion

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {
		gameManager	= GameObject.Find ( "/_GameManager" );
	}

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	private void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.CompareTag( "Player" ) ) {

			switch( this.gameObject.tag ) {		
				// コイン.
				case( "Coin" ):
					// コイン取得音再生.
					AudioSource.PlayClipAtPoint( CoinClips, transform.position );
					// 点数加算.
					gameManager.SendMessage( "addScore", Score );
					break;

				// 巨大化アイテム.
				case ( "Gigantic" ):
					// コイン取得音再生.
					AudioSource.PlayClipAtPoint( CoinClips, transform.position );
					// 点数加算.
					gameManager.SendMessage( "addScore", Score );
					// プレイヤー巨大化.
					gameManager.SendMessage( "SetPlayerGigantic" );
					break;
			}

			// Destroy.
			Destroy ( this.gameObject );
		}
	}
}
