using UnityEngine;
using System.Collections;

using Audio;
using PlayRunningGame.Player;

public class EnemyDead : MonoBehaviour {

	/// <summary>
	/// Destroies the object.
	/// </summary>
	private void DestroyObj( ) {
		// 敵キャラ倒されたSE再生.
		AudioManager.Instance.PlaySE( AudioConfig.EnemyDie );
		// 敵キャラ消滅パーティクル演出.
		PrefabPoolManager.Instance.instantiatePrefab( "CFXM3_Hit_SmokePuff", transform.parent.gameObject.transform.position, Quaternion.identity );
		// 敵キャラ消滅.
		Destroy ( this.transform.parent.gameObject );
	}
}