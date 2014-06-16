using UnityEngine;
using System.Collections;

public class LocalNotificationInspector : MonoBehaviour {

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake() {
		DontDestroyOnLoad( this );
	}

	/// <summary>
	/// アプリをタスク切らず再度起動した場合、バッヂアイコン非表示設定.
	/// </summary>
	/// <param name="pauseStatus">If set to <c>true</c> pause status.</param>
	private void OnApplicationPause( bool pauseStatus ) {
		if ( false == pauseStatus ) {
			OnBuild();
		}
	}

	/// <summary>
	/// Raises the build event.
	/// </summary>
	public static void OnBuild( ) {
		// 起動時Push通知のバッヂアイコンを非表示にする.
		LocalNotificationPlugin.SetLocalPushBadgeIconNonDisplay( );
	}
}
