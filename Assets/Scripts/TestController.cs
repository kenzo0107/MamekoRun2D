using UnityEngine;
using System.Collections;

using Notification;

public class TestController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int tag;
		string mes;
		tag	= (int)LocalNotificationConfig.LocalNotificationType.StaminaUp;
		mes	= LocalNotificationConfig.GetLocalNotificationMessage( tag );
		LocalNotificationPlugin.reserveLocalPush( mes, 10, tag );
		Debug.Log ( "(^-^)" );
	}
}
