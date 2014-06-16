using UnityEngine;

using System;
using System.Collections;
using System.Runtime.InteropServices;

public class LocalNotificationPlugin : MonoBehaviour {

#if UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern void reserveLocalNotification( string message,int infotime,int tag );
	[DllImport("__Internal")]
	private static extern void cancelLocalNotification( int tag );
	[DllImport("__Internal")]
	private static extern void setBadgeIconNonDisplay( );
	[DllImport("__Internal")]
	private static extern int getApplicationIconBadgeCountNumber( );
#elif UNITY_ANDROID
	private static AndroidJavaObject alartSender = null;
	private static AndroidJavaClass unityPlayer = null;
	private static AndroidJavaObject activity = null;
	
	private static void androidLocalNotificationInit(){
		alartSender = new AndroidJavaObject ( Config.PackageName + ".AlartSender" );
		unityPlayer = new AndroidJavaClass( "com.unity3d.player.UnityPlayer" );
		activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
	}

#endif
	/// <summary>
	/// ローカルPush予約処理.
	/// 同タグの場合でも複数回送付可能.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="infotime">Infotime.</param>
	/// <param name="tag">Tag.</param>
	public static void reserveLocalPush( string message, int infotime, int tag ) {

		// Platform毎に処理分け.
		switch( Application.platform ) {
			case ( RuntimePlatform.OSXEditor ):
				break;

			case ( RuntimePlatform.Android ):
#if UNITY_ANDROID
				Debug.Log ( String.Format( "message:{0}, infotime:{1}, tag:{2}", message, infotime, tag ) );
				androidLocalNotificationInit();
				using(alartSender) {
					// Android用ローカル通知を実行。ルームは秒単位の値で定義されているのでミリ秒に変換（*1000）を行う。
					alartSender.CallStatic( "localPushInJava", activity, Config.PackageName, 
					                       "", "", "app_icon", "",  message, (long)(infotime*1000), tag, "", true );
				}
#endif
				break;

			case ( RuntimePlatform.IPhonePlayer ):
				Debug.Log ( String.Format( "message:{0}, infotime:{1}, tag:{2}", message, infotime, tag ) );
#if UNITY_IPHONE
				reserveLocalNotification( message, infotime, tag );
#endif
				break;
		}
	}

	/// <summary>
	/// ローカルPush予約処理（上書き）.
	/// 同タグの場合、予約済み通知をキャンセルし新たに設定し直す.
	/// </summary>
	/// <param name="message">Message.</param>
	/// <param name="infotime">Infotime.</param>
	/// <param name="tag">Tag.</param>
	public static void reserveLocalPushUpdate( string message, int infotime, int tag  ) {

		// Platform毎に処理分け.
		switch( Application.platform ) {
		case ( RuntimePlatform.OSXEditor ):
			return;
			break;
			
		case ( RuntimePlatform.Android ):
#if UNITY_ANDROID
			androidLocalNotificationInit();
			using(alartSender) {
				// Android用ローカル通知をキャンセル
				cancelLocalPush( tag );
				// Android用ローカル通知を実行。ルームは秒単位の値で定義されているのでミリ秒に変換（*1000）を行う。
				alartSender.CallStatic( "localPushInJava", activity, Config.PackageName, 
				                       "", "", "app_icon", "",  message, (long)(infotime* 1000), tag, "", true );
			}
#endif
			break;
			
		case ( RuntimePlatform.IPhonePlayer ):
			Debug.Log ( String.Format( "message:{0}, infotime:{1}, tag:{2}", message, infotime, tag ) );
#if UNITY_IPHONE
			cancelLocalPush( tag );
			reserveLocalNotification( message, infotime, tag );
#endif
			break;
		}
	}

	/// <summary>
	/// ローカルPushキャンセル.
	/// </summary>
	/// <param name="tag">Tag.</param>
	public static void cancelLocalPush( int tag ) {

		// Platform毎に処理分け.
		switch( Application.platform ) {
			case ( RuntimePlatform.OSXEditor ):
				return;
				break;
				
			case ( RuntimePlatform.Android ):
#if UNITY_ANDROID
				Debug.Log ( String.Format( "message:{0}", tag ) );
				androidLocalNotificationInit();
				using(alartSender) {
					// Android用ローカル通知をキャンセル
					alartSender.CallStatic("localPushCancelInJava", activity, tag );
				}
#endif
				break;
				
			case ( RuntimePlatform.IPhonePlayer ):
				Debug.Log ( String.Format( "message:{0}", tag ) );
#if UNITY_IPHONE
				cancelLocalNotification( tag );
#endif
				break;
		}
	}

	/// <summary>
	/// バッジアイコンを非表示するよう設定.
	/// </summary>
	public static void SetLocalPushBadgeIconNonDisplay( ) { 
		switch( Application.platform ) {
			case ( RuntimePlatform.OSXEditor ):
				return;
				break;
				
			case ( RuntimePlatform.Android ):
#if UNITY_ANDROID

#endif
				break;
				
			case ( RuntimePlatform.IPhonePlayer ):
#if UNITY_IPHONE
				setBadgeIconNonDisplay( );
#endif
				break;
		}
	}

	/// <summary>
	/// Gets the local push badge count.
	/// </summary>
	public static int GetLocalPushBadgeCount( ) { 

		int bagdeCount;
		bagdeCount	= 0;

		switch( Application.platform ) {
			case ( RuntimePlatform.OSXEditor ):
				break;
				
			case ( RuntimePlatform.Android ):
#if UNITY_ANDROID

#endif
				break;
				
			case ( RuntimePlatform.IPhonePlayer ):
#if UNITY_IPHONE
				bagdeCount	= getApplicationIconBadgeCountNumber( );
#endif
				break;
		}

		return bagdeCount;
	}

}
