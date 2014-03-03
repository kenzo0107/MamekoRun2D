using UnityEngine;
using System.Collections;

namespace PlayRunningGame {

	public class GameManager : MonoBehaviour {

		#region event
		public delegate void RollUpDown();
		public static event RollUpDown StartRollUpDown;
		#endregion event

		#region public members.
		public Camera		ChaserCamera;
		public GameObject	DeadLineY;
		public GameObject	PlayerObj;
		public GameObject	DistanceScore;
		#endregion

		#region private members.
		private GameObject			_Anchor;
		private Vector3				_playerPosition;
		private PlayRunningGame.Player.PlayerController	_playerController;
		private ChasePlayer			_chasePlayer;
		private UILabel				_uiLabelDistanceScore;
		private bool				_executedFlag	= false;
		private bool				_isCameraEnableChasePlayer;
		#endregion

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake( ) {
			_Anchor					= GameObject.Find ( "Anchor" );
			_playerController		= PlayerObj.GetComponent<PlayRunningGame.Player.PlayerController>();
			_chasePlayer			= ChaserCamera.GetComponent<ChasePlayer>();
			_playerPosition			= PlayerObj.transform.position;
			_uiLabelDistanceScore	= DistanceScore.GetComponent<UILabel>();
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		void FixedUpdate( ) {

			if ( PlayerObj != null ) {

				_playerPosition			= PlayerObj.transform.position;

				if ( _playerPosition.x > 0 ) {
					_uiLabelDistanceScore.text	= (string)System.Convert.ToString( Mathf.Floor( _playerPosition.x / 2 ) );
				}

				// 追跡するプレイヤーのX軸ポジション  が一定位置まで来た場合に追従開始.
				if ( false == _isCameraEnableChasePlayer ) {

					if ( _playerPosition.x >= ChaserCamera.transform.position.x + Player.PlayerConfig.PlayerPositionByCamera ) {
						//  カメラのプレイヤー追跡処理.
						_isCameraEnableChasePlayer	= true;
						// カメラの追跡速度をプレイヤー速度に合わせる.
						_chasePlayer.SpeedRight		= _playerController.SpeedRight;

						// 左端デッドライン　プレイヤーがぶつかるとプレイヤー消滅.
						DeadLineY.SetActive( true );
						//  ロール Up & Down.
						StartRollUpDown();

						// プレイヤー操作可能.
						_playerController.IsController	= true;
					}
				}
				else {
					if( _playerPosition.x < ChaserCamera.transform.position.x + Player.PlayerConfig.PlayerPositionByCamera ) {
						PlayerObj.SendMessage( "SpeedUp" );
					}
					else {
						PlayerObj.SendMessage( "setCurrentSpeed" );
					}
				}
			}
		}

		/// <summary>
		/// プレイヤーが死判定.
		/// 
		/// BGMストップ.
		/// カメラの移動減速しストップ,
		/// </summary>
		void setPlayerDie( ) {
			if ( false == _executedFlag ) {
				_executedFlag	= true;

				// BGM ストップ.
				Audio.AudioManager.Instance.StopBGM();

				// プレイヤー追跡カメラをブレーキ.
				_chasePlayer.SendMessage( "SetBrakeSpeed" );

				//  ゲームオーバーボタン生成.
				_instantiateBtnGameOver( );
			}
		}

		/// <summary>
		/// _instantiateBtnGameOver
		/// </summary>
		void _instantiateBtnGameOver( ) {
			GameObject _objBtnGameOver	= (GameObject)Instantiate ( Resources.Load( "Common/BtnGameOver" ), Vector3.zero, Quaternion.identity );
			_objBtnGameOver.transform.parent		= _Anchor.transform;
			_objBtnGameOver.transform.localScale	= Vector3.one;
			_objBtnGameOver.transform.localPosition	= Vector3.zero;
		}

		/// <summary>
		/// Retire this instance.
		/// </summary>
		void RetireGame( ) {
			Destroy ( PlayerObj );
			this.SendMessage( "OnClickButton" );
		}

		#region OnGUI
		/*	
		/// <summary>
		/// Raises the GU event.
		/// </summary>
		void OnGUI( ) {
			if ( Config.IS_DEBUG ) {
				GUI.Box( new Rect(5, 5, 300, 100), string.Format( "_isCameraEnableChasePlayer : {0} \n", _isCameraEnableChasePlayer ) );
			}
		}
	*/
		#endregion OnGUI
	}
}