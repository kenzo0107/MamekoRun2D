using UnityEngine;
using System.Collections;

public class EnemyDead : MonoBehaviour {

	/// <summary>
	/// Raises the collision enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D( Collision2D coll ) {

		if ( coll.gameObject.name.Contains( "Player" ) ) {

			// 敵キャラ倒されたSE再生.
			Audio.AudioManager.Instance.PlaySE( "se_scratchy_jump_rnd_01" );

			// 敵キャラ消滅パーティクル演出.
			PrefabPoolManager.Instance.instantiatePrefab( "CFXM3_Hit_SmokePuff", this.gameObject.transform.position, Quaternion.identity );
//			GameObject objEffect	= (GameObject) Instantiate( Resources.Load ( "JMO Assets/Cartoon FX/CFX3 Prefabs (Mobile)/Misc/CFXM3_Hit_SmokePuff" ), Vector3.zero, Quaternion.identity );

			// 敵キャラ消滅.
			Destroy ( this.transform.parent.gameObject );
		}
	}
}
