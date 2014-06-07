using UnityEngine;

using System;
using System.Collections;

using PlayRunningGame.Player;
using Audio;

namespace PlayRunningGame {

	public class GameManager : MonoBehaviour {

		#region event
		public delegate void RollUpDown();
		public static event RollUpDown StartRollUpDown;
		#endregion event

		#region public members.
		/// <summary> 追跡カメラ.</summary>
		public Camera		ChaserCamera;
		/// <summary> デッドラインY軸 （スクリーン左端でプレイヤーと衝突するとプレイヤーをDead状態にする）.</summary>
		public GameObject	DeadLineY;
		/// <summary> Playerオブジェクト.</summary>
		public GameObject	PlayerObj;
		/// <summary> 距離スコア.</summary>
		public GameObject	DistanceScore;
		/// <summary> 背景0.</summary>
		public GameObject	Bg0;
		/// <summary>フィーバー中か判定.</summary>
		public bool		isFever	= false;
		#endregion

		#region private members.
		/// <summary>Anchorオブジェクト.</summary>
		private GameObject			Anchor;
		/// <summary>プレイヤー LocalPosition.</summary>
		private Vector3				playerPosition;
		/// <summary>プレイヤーコントローラー.</summary>
		private PlayerController	playerController;
		/// <summary>プレイヤー追跡.</summary>
		private ChasePlayer			chasePlayer;

		private Vector3				chasePlayerCameraPosition;
		/// <summary>距離スコアLabel.</summary>
		private UILabel				uiLabelDistanceScore;
		/// <summary>実行フラグ.</summary>
		private bool				executedFlag	= false;
		/// <summary>Cameraのプレイヤー追跡可能判定.</summary>
		private bool				isCameraEnableChasePlayer;
		/// <summary>chasePlayerCamera最大ローカルポジション(X軸).</summary>
		private float				maxChasePlayerCameraLocalPositionX;

		private FloorMapManager				floorMapManager;

		private Transform	playerTransform;
		private Transform	chasePlayerCameraTransform;
		#endregion

		/// <summary>
		/// Awake this instance.
		/// </summary>
		private void Awake( ) {
			Anchor						= GameObject.Find ( "Anchor" );
			playerController			= PlayerObj.GetComponent<PlayerController>();
			playerTransform				= PlayerObj.transform;
			chasePlayer					= ChaserCamera.GetComponent<ChasePlayer>();
			chasePlayerCameraTransform	= ChaserCamera.transform;
			uiLabelDistanceScore		= DistanceScore.GetComponent<UILabel>();
			floorMapManager				= this.GetComponent<FloorMapManager>();
		}

		/// <summary>
		/// Start this instance.
		/// </summary>
		private IEnumerator Start ( ) {
			yield return StartCoroutine( GameSetting() );
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
//		private void FixedUpdate( ) {
		private IEnumerator GameSetting( ) {

			while ( true ) {

				chasePlayerCameraPosition	= chasePlayerCameraTransform.position;

				if ( maxChasePlayerCameraLocalPositionX < Mathf.Floor( chasePlayerCameraPosition.x ) ) {
					maxChasePlayerCameraLocalPositionX	= Mathf.Floor( chasePlayerCameraPosition.x );
//					Debug.Log ( "maxChasePlayerCameraLocalPositionX:"+maxChasePlayerCameraLocalPositionX );
					floorMapManager.InstantiateFloor( (int)maxChasePlayerCameraLocalPositionX );
				}
				
				if ( null != PlayerObj ) {
					playerPosition			= playerTransform.position;

					if ( playerPosition.x > 0 ) {
						uiLabelDistanceScore.text	= Convert.ToString( Mathf.Floor( playerPosition.x / 2 ) );
					}

					// 追跡するプレイヤーのX軸ポジション  が一定位置まで来た場合に追従開始.
					if ( false == isCameraEnableChasePlayer ) {

						if ( playerPosition.x >= ChaserCamera.transform.position.x + PlayerConfig.PlayerPositionByCamera ) {
							//  カメラのプレイヤー追跡処理.
							isCameraEnableChasePlayer	= true;
							// カメラの追跡速度をプレイヤー速度に合わせる.
							chasePlayer.SpeedRight		= playerController.SpeedRight;

							// 左端デッドライン　プレイヤーがぶつかるとプレイヤー消滅.
							DeadLineY.SetActive( true );
							//  ロール Up & Down.
							StartRollUpDown();

							// プレイヤー操作可能.
							playerController.IsController	= true;
						}
					}
					else {
						if( playerPosition.x < ChaserCamera.transform.position.x + PlayerConfig.PlayerPositionByCamera ) {
							PlayerObj.SendMessage( "SpeedUp" );
						}
						else {
							PlayerObj.SendMessage( "SetCurrentSpeed" );
						}
					}

				}
				yield return new WaitForSeconds( 0.15f );
			}
		}

		/// <summary>
		/// フィーバー状態に設定.
		/// </summary>
		private void SetFever( ) {
			Debug.Log ( "Fever!!!!!!!!!!!!!!!" );
			AudioManager.Instance.PlayBGM( AudioConfig.PlayRunningUnrivaled );
		}

		/// <summary>
		/// プレイヤーが死判定.
		/// 
		/// BGMストップ.
		/// カメラの移動減速しストップ,
		/// </summary>
		private void SetPlayerDie( ) {

			if ( false == executedFlag ) {

				executedFlag	= true;

				// BGM ストップ.
				AudioManager.Instance.StopBGM();

				// プレイヤー追跡カメラをブレーキ.
				chasePlayer.SendMessage( "SetBrakeSpeed" );
				// 背景のスクロールブレーキ.
				Bg0.SendMessage( "BrakeScroll" );

<<<<<<< HEAD
				// スコアポップアップ表示.
				InstantiatePopupScore( );
=======
				//  ゲームオーバーボタン生成.
				InstantiateBtnGameOver( );
			}
		}

		/// <summary>
		/// InstantiateBtnGameOver
		/// </summary>
		 private void InstantiateBtnGameOver( ) {
			GameObject _objBtnGameOver	= (GameObject)Instantiate ( Resources.Load( "Common/BtnGameOver" ), Vector3.zero, Quaternion.identity );
			_objBtnGameOver.transform.parent		= Anchor.transform;
			_objBtnGameOver.transform.localScale	= Vector3.one;
			_objBtnGameOver.transform.localPosition	= Vector3.zero;
>>>>>>> FETCH_HEAD
		}

		/// <summary>
		/// Retire this instance.
		/// </summary>
		void RetireGame( ) {
			Destroy ( PlayerObj );
			PlayerObj	= null;
			this.SendMessage( "OnClickButton" );
		}

		#region OnGUI
/*	
		/// <summary>
		/// Raises the GU event.
		/// </summary>
		void OnGUI( ) {
			if ( Config.IS_DEBUG ) {
				GUI.Box( new Rect(5, 5, 300, 100), string.Format( "isCameraEnableChasePlayer : {0} \n", isCameraEnableChasePlayer ) );
			}
		}
*/
		#endregion OnGUI
	}
}