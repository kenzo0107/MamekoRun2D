using UnityEngine;
using System.Collections;

using PlayRunningGame;

public class ConversationManager : MonoBehaviour {

	#region private members.
	private int			talkStep	= 0;
	private string[]	conversationList	= new string[3];
	private GameObject	tellerLeft;
	private GameObject	tellerRight;
	private UILabel		uiLabelMessageLeft;
	private UILabel		uiLabelMessageRight;
	private MoveObject	balloonMoveObject;
	private GameManager gameManager;
	private int			conversationId;
	#endregion private members.

	#region property
	public	int			ConversationId { set { conversationId	= value; } }
	#endregion

/// <summary>
/// Awake this instance.
/// </summary>
	private void Awake( ) {
		gameManager	= GameObject.Find ( "_GameManager" ).GetComponent<GameManager>();
		tellerLeft	= transform.FindChild( "TellerLeft" ).gameObject;
		tellerRight	= transform.FindChild( "TellerRight" ).gameObject;
		balloonMoveObject	= transform.FindChild( "Talk/Balloon" ).GetComponent<MoveObject>();
		uiLabelMessageLeft		= balloonMoveObject.transform.FindChild( "MessageLeft" ).gameObject.GetComponent<UILabel>();
		uiLabelMessageRight		= balloonMoveObject.transform.FindChild( "MessageRight" ).gameObject.GetComponent<UILabel>();
	}

	/// <summary>
	/// 次の話へ.
	/// </summary>
	private void NextTalk( ) {

		conversationList	= ConversationConfig.GetConversation( conversationId, talkStep );
		talkStep	+= 1;

		// 会話データが存在している場合.
		if ( null != conversationList ) {
			SetTeller ( conversationList[0], tellerLeft );
			SetTeller ( conversationList[1], tellerRight );
			SetBalloon( conversationList );
			bool isLeftTeller	= !string.IsNullOrEmpty( conversationList[0] );
			uiLabelMessageLeft.text		= ( isLeftTeller )?	ColorConfig.Pink	+ conversationList[2]	:	null;
			uiLabelMessageRight.text	= ( isLeftTeller )?	null										:	ColorConfig.Black	+ conversationList[2];
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

		balloonMoveObject.SetRotation( new Vector2( 0f, from ), new Vector2( 0f, to ), 0.3f );
		balloonMoveObject.IsStart	= true;
	}

	/// <summary>
	/// Raises the finish event.
	/// </summary>
	private void OnFinish( ) {
		Debug.Log ( "OnFinish" );
		tellerLeft.SetActive( true );
		tellerRight.SetActive( true );

		ReleaseChildObj( tellerLeft );
		ReleaseChildObj( tellerRight );

		talkStep	= 0;
		gameManager.SendMessage( "EndConversation" );
	}

	/// <summary>
	/// Releases the child object.
	/// </summary>
	/// <param name="obj">Object.</param>
	private void ReleaseChildObj( GameObject obj ) {
		Transform[]	transformList;
		transformList	= obj.GetComponentsInChildren<Transform>();
		foreach ( Transform child in transformList ) {
			PrefabPoolManager.Instance.ReleasePrefab( child.gameObject );
		}
	}
}
