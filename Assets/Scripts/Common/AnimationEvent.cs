 using UnityEngine;
using System.Collections;

using PlayRunningGame;

public class AnimationEvent : MonoBehaviour {

	#region private members.
	private GameManager	gameManager; 
	#endregion private members.

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {
		gameManager	= GameObject.Find ( "_GameManager" ).GetComponent<GameManager>();
	}

	/// <summary>
	/// Destroies the object.
	/// </summary>
	private void DestroyObj( ) {
		Destroy( this.gameObject );
	}

	/// <summary>
	/// Sets the water plane.
	/// </summary>
	private void SetWaterPlane( ) {
		gameManager.SetActiveWaterPlane( true );
	}

	/// <summary>
	/// Changes to sea stage.
	/// </summary>
	private void ChangeToSeaStage( ) {
		gameManager.ChangeToSeaStage();
	}

	/// <summary>
	/// Sets the un active.
	/// </summary>
	private void SetUnActive( ) {
		this.gameObject.SetActive( false );
	}
}
