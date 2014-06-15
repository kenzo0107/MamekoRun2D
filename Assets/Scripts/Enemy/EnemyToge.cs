using UnityEngine;
using System.Collections;

public class EnemyToge : MonoBehaviour {

	private enum enemyTogeStatus {
		 Move
		,Idle
	}
	private enemyTogeStatus status;

	private void Awake( ) {
		status	= enemyTogeStatus.Move;
	}
}
