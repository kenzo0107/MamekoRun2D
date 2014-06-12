using UnityEngine;
using System.Collections;

using Audio;
using PlayRunningGame.Player;

public class EnemyDead : MonoBehaviour {

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	private void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.tag.Contains( "Player" ) ) {

			Debug.Log ( "OnTriggerEnter2D  coll.gameObject.tag:" + coll.gameObject.tag );

			// 敵キャラ倒されたSE再生.
			AudioManager.Instance.PlaySE( AudioConfig.EnemyDie );
			
			// 敵キャラ消滅パーティクル演出.
			PrefabPoolManager.Instance.instantiatePrefab( "CFXM3_Hit_SmokePuff", transform.parent.gameObject.transform.position, Quaternion.identity );

			// 敵キャラ消滅.
			Destroy ( this.transform.parent.gameObject );
		}
	}

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	private void OnCollisionEnter2D( Collision2D coll ) {
		
		if ( coll.gameObject.tag.Contains( "Player" ) ) {

			Debug.Log ( "OnCollisionEnter2D  coll.gameObject.tag:" + coll.gameObject.tag );

			// 敵キャラ倒されたSE再生.
			AudioManager.Instance.PlaySE( AudioConfig.EnemyDie );
			
			// 敵キャラ消滅パーティクル演出.
			PrefabPoolManager.Instance.instantiatePrefab( "CFXM3_Hit_SmokePuff", transform.parent.gameObject.transform.position, Quaternion.identity );
			
			// 敵キャラ消滅.
			Destroy ( this.transform.parent.gameObject );
		}
	}
}