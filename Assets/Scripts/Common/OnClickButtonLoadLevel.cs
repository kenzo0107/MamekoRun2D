using UnityEngine;
using System.Collections;

public class OnClickButtonLoadLevel : MonoBehaviour {

	#region public members.
	public Config.SceneList Scene;
	#endregion public members.

	#region private members.
	private GameObject	LoadLevelManager;
	#endregion

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake( ) {
		LoadLevelManager	= GameObject.Find ( "LoadLevelManager" ).gameObject;
	}

	/// <summary>
	/// Raises the click button event.
	/// </summary>
	void OnClickButton( ) {
		LoadLevelManager.SendMessage( "loadLevel", (int)Scene );
	}
}
