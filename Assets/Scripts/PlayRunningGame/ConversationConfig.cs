 using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

public class ConversationConfig {

	///<summary>キャラクター.</summary>
	public enum Character {
		Player,
		LegendaryWizard
	}

	/// <summary>プレイヤー移動距離の到達地点毎の会話リスト.</summary>
	public static readonly IDictionary<int, string[][]>Distance2ConversationList	= new Dictionary<int, string[][]> {
		{
			5,
			new string[][] {
				new string[3] { "Player",	null, 				"あ、あなたは誰ですか？\nあわわ..." },
				new string[3] { null,		"LegendaryWizard",	"わしは仙人じゃ！" },
				new string[3] { "Player",	null,				"そうですか、では！" },
			}
		},
		{
			7,
			new string[][] {
				new string[3] { null,		"LegendaryWizard",	"ちょ、ちょ待てよ！" },
				new string[3] { "Player",	null,				"え、何ですか？" },
				new string[3] { null,		"LegendaryWizard",	"これから\n長い旅のはじまりじゃ\n心してかかれよ！" },
				new string[3] { "Player",	null,				"は、はい！\n\n（一体誰？？\n見たことあるような…）" },
			}
		},
	};

	/// <summary>
	///  会話が存在するか判定.
	/// </summary>
	/// <returns><c>true</c> if is talk map the specified id; otherwise, <c>false</c>.</returns>
	/// <param name="id">会話ID.</param>
	public static bool IsConversation( int id ) {
		return ( Distance2ConversationList.ContainsKey( id ) );

	}

	/// <summary>
	/// Gets the conversation.
	/// </summary>
	/// <returns>The conversation.</returns>
	/// <param name="Id">Identifier.</param>
	public static string[] GetConversation( int id, int step ) {
		if ( step >= Distance2ConversationList[ id ].Length ) {
			return null;
		}

		Debug.Log ( String.Format( "TalkMap:[{0}][0]={1}, [1]={2}, [2]={3}" , step,  Distance2ConversationList[ id ][ step ][0], Distance2ConversationList[ id ][ step ][1], Distance2ConversationList[ id ][ step ][2] ) );
		return Distance2ConversationList[ id ][ step ];
	}
}
