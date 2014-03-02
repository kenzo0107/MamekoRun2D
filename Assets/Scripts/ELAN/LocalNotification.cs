using UnityEngine;
using System.Collections;

public class LocalNotification : MonoBehaviour {
	
	private string title = "Enter title";
	private string message = "Enter message";
	private string delay = "0";
	private string rep = "0";
	
	private string error = "Set the repeating box to 0 or negative for one off notifications";
		
	void OnGUI () {
		// Schedule notifications GUI
		GUI.Box(new Rect(10,10,340,300), "ELAN Demo");

		if(GUI.Button(new Rect(20,40,120,40), "Send")) {
			long d;
			if(long.TryParse(delay, out d)) {
				long r;
				if(long.TryParse(rep, out r)) {
					if(r <= 0) ELANManager.SendNotification(title,message,d);
					else ELANManager.ScheduleRepeatingNotification(title,message,d,r);
					error = "";
				} else error = "Repetition must be an integer!";
			}else error = "Delay must be an integer!";
		}
		title = GUI.TextField (new Rect(20,90,120,40), title, 15);
		message = GUI.TextField (new Rect(20,140,120,40), message, 15);
		GUI.Label (new Rect(20,190,120,40),"Delay");
		delay = GUI.TextField (new Rect(120,190,120,40), delay, 15);
		GUI.Label (new Rect(20,240,120,40),"Repetition");
		rep = GUI.TextField (new Rect(120,240,120,40), rep, 15);
		if(GUI.Button(new Rect(20,320,120,40), "Exit")) {
			Application.Quit();
		}
		
		GUI.Label (new Rect(20,380,300,40),error);
		
		// Cancel scheduled notification GUI
		GUI.Box(new Rect(410,10,300,150), "Cancel notifications");

		if(GUI.Button(new Rect(420,40,140,40), "Cancel repeating")) {
			ELANManager.CancelRepeatingNotification();
		}
		if(GUI.Button(new Rect(420,90,140,40), "Cancel ALL")) {
			ELANManager.CancelAllNotifications();
		}
	}
}
