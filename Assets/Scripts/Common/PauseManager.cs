using UnityEngine;
using System.Collections;

public class PauseManager : MonoBehaviour {

	#region public members.
	public GameObject Popup;
	public GameObject Player;
	#endregion public members.

	/// <summary>
	/// Sets the popup active.
	/// </summary>
	void setPopupActive( ) {
		if ( false == Popup.activeSelf ) {
			//  時間軸停止 .
			Time.timeScale	= 0;
			// プレイヤー一時停止.
			Player.SendMessage( "setIsController", false );
			// BGM停止.
			Audio.AudioManager.Instance.PauseBGM( );
			// ポップアップ表示.
			Popup.SetActive( true );
		}
		else {
			//  時間軸通常.
			Time.timeScale	= 1;
			// プレイヤーコントロールOK.
			Player.SendMessage( "setIsController", true );
			// BGM再開.
			Audio.AudioManager.Instance.ReStart( );
			// ポップアップ非表示.
			Popup.SetActive( false );
		}
	}
}
