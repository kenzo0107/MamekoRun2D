using UnityEngine;
using System.Collections;

public class TotalGameManager : SingletonMonoBehaviour<TotalGameManager> {

	/// <summary>The audio manager.</summary>
	private static GameObject audioManager;
	/// <summary>The debug manager.</summary>
	private static GameObject debugManager;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {

		if( this != Instance ) {
			Destroy(this);
			return;
		}
		DontDestroyOnLoad(this.gameObject);

		// AudioManager.
		audioManager	= getGameObj( "AudioManager" );

		if ( true == Config.IS_DEBUG ) {
			// DebugManager.
			debugManager	= getGameObj( "DebugManager" );
		}
	}

	/// <summary>
	/// Gets the game obje.
	/// </summary>
	/// <returns>The game obje.</returns>
	/// <param name="objName">Object name.</param>
	private GameObject getGameObj( string objName ) {

		GameObject objManager	= GameObject.Find( objName );

		if ( null == objManager ) {
			objManager	= (GameObject)Instantiate( Resources.Load ( "Common/" + objName, typeof(GameObject) ) );
			objManager.name	= objName;
			objManager.transform.parent	= this.gameObject.transform;
		} 

		return objManager;
	}

}
