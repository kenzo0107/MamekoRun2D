using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

	#region public members.
	public GameObject Popup;
	#endregion public members.

	#region private members.
	private GameObject objPlayer;
	#endregion private members.

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {
		objPlayer	= GameObject.Find( "Player" );
	}

	/// <summary>
	/// Sets the popup active.
	/// </summary>
	private void setPopupActive( ) {
		if ( false == Popup.activeSelf ) {
			//  時間軸停止 .
			Time.timeScale	= 0;
			// プレイヤー一時停止.
			objPlayer.SendMessage( "setIsController", false );
			// BGM停止.
			Audio.AudioManager.Instance.PauseBGM( );
			// ポップアップ表示.
			Popup.SetActive( true );
		}
		else {
			//  時間軸通常.
			Time.timeScale	= 1;
			// プレイヤーコントロールOK.
			objPlayer.SendMessage( "setIsController", true );
			// BGM再開.
			Audio.AudioManager.Instance.ReStart( );
			// ポップアップ非表示.
			Popup.SetActive( false );
		}
	}
}
