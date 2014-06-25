using UnityEngine;
using System.Collections;

public class ConversationManager : MonoBehaviour {

	#region private members.
	private int			talkId;
	private string[]	conversationList	= new string[3];
	private GameObject	tellerLeft;
	private GameObject	tellerRight;
	private UILabel		uiLabelMessage;
	private Transform	transformBalloon;
	#endregion private members.

	/// <summary>
	/// Initializes a new instance of the <see cref="ConversationManager"/> class.
	/// </summary>
	private void Awake( ) {
		tellerLeft	= transform.FindChild( "TellerLeft" ).gameObject;
		tellerRight	= transform.FindChild( "TellerRight" ).gameObject;
		uiLabelMessage		= transform.FindChild( "Talk/Message" ).gameObject.GetComponent<UILabel>();
		transformBalloon	= transform.FindChild( "Talk/Balloon" ).gameObject.transform;
	}

	/// <summary>
	/// 次の話へ.
	/// </summary>
	private void NextTalk( ) {

		string message;

		Debug.Log( "NextTalk" );

		if ( null == talkId ) {
			talkId	= 0;
		}
		conversationList	= ConversationConfig.GetConversation( talkId );
		talkId	+= 1;

		// 会話データが存在している場合.
		if ( null != conversationList ) {
			SetTeller ( conversationList[0], tellerLeft );
			SetTeller ( conversationList[1], tellerRight );
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

			if ( "tellerLeft" == objParent.name ) {
				transformBalloon.rotation = Quaternion.Euler( new Vector2( 0f, 180f ) );
			}
			else {
				transformBalloon.rotation = Quaternion.Euler( Vector2.zero );
			}
		}
	}

	/// <summary>
	/// Raises the finish event.
	/// </summary>
	private void OnFinish( ) {
		Debug.Log ( "OnFinish" );
	}
}
