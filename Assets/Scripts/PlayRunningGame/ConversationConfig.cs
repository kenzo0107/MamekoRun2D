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
			20,
			new string[][] {
				new string[3] { "PlayerInConversation",	null, 								"あ、あなたは誰ですか？\nあわわ..." },
				new string[3] { null,					"LegendaryWizardInConversation",	"わしは仙人じゃ！" },
				new string[3] { "PlayerInConversation",	null,								"そうですか、では！" },
			}
		},
		{
			21,
			new string[][] {
				new string[3] { null,					"LegendaryWizardInConversation",	"ちょ、ちょ待てよ！" },
				new string[3] { "PlayerInConversation",	null,								"え、何ですか？" },
				new string[3] { null,					"LegendaryWizardInConversation",	"これから\n長い旅のはじまりじゃ\n心してかかれよ！" },
				new string[3] { "PlayerInConversation",	null,								"は、はい！" },
				new string[3] { "PlayerInConversation",	null,								"一体誰？？\n見たことあるような…" },
			}
		},
		{
			30,
			new string[][] {
				new string[3] { null,					"CloudInConversation",				"おや、マメコかい？" },
				new string[3] { "PlayerInConversation",	null,								"あっクラウド先生！" },
				new string[3] { null,					"CloudInConversation",				"ふふふ、今日もうさぎだな" },
				new string[3] { "PlayerInConversation",	null,								"（腹立つな…）" },
				new string[3] { null,					"CloudInConversation",				"この先大シケだよ\n気をつけるといい" },
				new string[3] { "PlayerInConversation",	null,								"はいっ\nありがとうございます！" },
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
		return Distance2ConversationList[ id ][ step ];
	}
}
