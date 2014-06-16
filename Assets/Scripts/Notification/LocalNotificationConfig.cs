using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Notification {

	public class LocalNotificationConfig {

		///<summary>ローカルPushタイプ</summary>
		public enum LocalNotificationType {
			 StaminaUp	= 1,
		}

		///<summary>ローカルPush文言リスト</summary>
		public static readonly IDictionary<int, string> LocalNotificationList = new Dictionary<int, string>() {
			 { (int)LocalNotificationType.StaminaUp,	"体力回復したよ♪新記録に挑戦しよう！" }
		};

		/// <summary>
		/// Gets the local notification message.
		/// </summary>
		/// <returns>The local notification message.</returns>
		/// <param name="id">Identifier.</param>
		public static string GetLocalNotificationMessage( int id ) {
			string mes	= null;
			if ( false == System.String.IsNullOrEmpty( LocalNotificationList[ id ] ) ) {
				mes	= LocalNotificationList[ id ];
			}
			return mes;
		}
	}
}