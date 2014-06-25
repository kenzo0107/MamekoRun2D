using UnityEngine;
using System.Collections;

using PlayRunningGame;

public class ConversationManager : MonoBehaviour {

	#region private members.
	private int			talkId	= 0;
	private string[]	conversationList	= new string[3];
	private GameObject	tellerLeft;
	private GameObject	tellerRight;
	private UILabel		uiLabelMessage;
	private MoveObject	balloonMoveObject;
	private GameManager gameManager;
	#endregion private members.

	/// <summary>
	/// Initializes a new instance of the <see cref="ConversationManager"/> class.
	/// </summary>
	private void Awake( ) {
		gameManager	= GameObject.Find ( "_GameManager" ).GetComponent<GameManager>();
		tellerLeft	= transform.FindChild( "TellerLeft" ).gameObject;
		tellerRight	= transform.FindChild( "TellerRight" ).gameObject;
		uiLabelMessage		= transform.FindChild( "Talk/Message" ).gameObject.GetComponent<UILabel>();
		balloonMoveObject	= transform.FindChild( "Talk/Balloon" ).GetComponent<MoveObject>();
	}

	/// <summary>
	/// 次の話へ.
	/// </summary>
	private void NextTalk( ) {

		Debug.Log( "NextTalk" );

		conversationList	= ConversationConfig.GetConversation( talkId );
		talkId	+= 1;

		// 会話データが存在している場合.
		if ( null != conversationList ) {
			// 左側語り手が指定されている場合.
			if ( false == string.IsNullOrEmpty( conversationList[0] ) ) {

			}
			SetTeller ( conversationList[0], tellerLeft );
			SetTeller ( conversationList[1], tellerRight );
			SetBalloon( conversationList ); 
			uiLabelMessage.text	= conversationList[2];
		}
		// 会話データが存在していない場合.
		else {
			OnFinish( );
		}
	}

	/// <summary>
	/// Sets the teller.
	/// </summary>
	/// <param name="objName">Object name.</param>
	/// <param name="objParent">Object parent.</param>
	private void SetTeller ( string objName, GameObject objParent ) {
		if ( true == string.IsNullOrEmpty( objName ) ) {
			objParent.SetActive( false );
		}
		else {
			if ( false == objParent.transform.FindChild( objName ) ) {
				PrefabPoolManager.Instance.instantiatePrefab( objName, transform.localPosition, Quaternion.identity, objParent.transform, Vector3.one );
			}
			objParent.SetActive( true );
		}
	}

	/// <summary>
	/// Sets the balloon.
	/// </summary>
	/// <param name="convList">Conv list.</param>
	private void SetBalloon( string[] convList ) {
		float	from;
		float	to;
		bool	isLeftTellerActive;

		isLeftTellerActive	= string.IsNullOrEmpty( convList[1] );

		from	= ( isLeftTellerActive )?	0		:	180f;
		to		= ( isLeftTellerActive )?	180f	:	0f;

		balloonMoveObject.SetRotation( new Vector2( 0f, from ), new Vector2( 0f, to ), 0.5f );
		balloonMoveObject.IsStart	= true;
	}

	/// <summary>
	/// Raises the finish event.
	/// </summary>
	private void OnFinish( ) {
		Debug.Log ( "OnFinish" );
		gameManager.SendMessage( "EndConversation" );
	}
}
