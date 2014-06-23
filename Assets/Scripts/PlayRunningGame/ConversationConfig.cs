using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationConfig {

	public enum Character {
		Player,
		LegendaryWizard
	}

	public List<Talk> TalkList;

	public class Talk {
		public string TellerLeft;
		public string TellerRight;
		public string Message;

		public Talk( string tellerLeft, string tellerRight, string message ) {
			TellerLeft	= tellerLeft;
			TellerRight	= tellerRight;
			Message		= message;
		}
	}

	public static readonly string[][] TalkMap	= new string[][] {
		new string[3] { "Player",	null, 				"あ、あなたは誰ですか？\nあわわ..." },
		new string[3] { null,		"LegendaryWizard",	"わしは仙人じゃ！" }
	};

	public static string[] GetConversation( int Id ) {
		if ( Id >= TalkMap.Length ) {
			return null;
		}

		Debug.Log ( String.Format( "TalkMap:[{0}][0]={1}, [1]={2}, [2]={3}" , Id,  TalkMap[Id][0], TalkMap[Id][1], TalkMap[Id][2] ) );
		return TalkMap[Id];
	}
}
