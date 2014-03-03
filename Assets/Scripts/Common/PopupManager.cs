using UnityEngine;
using System.Collections;

public class PopupManager : MonoBehaviour {

	#region public members.
	public GameObject Popup;
	#endregion public members.

	/// <summary>
	/// Sets the popup active.
	/// </summary>
	void setPopupActive( ) {
		if ( false == Popup.activeSelf ) {
			//  時間軸停止 .
			Time.timeScale	= 0;
			// BGM停止.
			Audio.AudioManager.Instance.PauseBGM( );
			// ポップアップ表示.
			Popup.SetActive( true );
		}
		else {
			//  時間軸通常.
			Time.timeScale	= 1;
			// BGM再開.
			Audio.AudioManager.Instance.ReStart( );
			// ポップアップ非表示.
			Popup.SetActive( false );
		}
	}
}
