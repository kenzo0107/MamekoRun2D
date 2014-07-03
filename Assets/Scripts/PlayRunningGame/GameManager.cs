using UnityEngine;

using System;
using System.Collections;

using PlayRunningGame;
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
		/// <summary>フィーバー背景.</summary>
		private GameObject			bgFever;
		/// <summary>プレイヤー追跡カメラLocalPosition.</summary>
		private Vector3				chasePlayerCameraPosition;
		/// <summary>距離スコアLabel.</summary>
		private UILabel				uiLabelDistanceScore;
		/// <summary>実行フラグ.</summary>
		private bool				executedFlag	= false;
		/// <summary>Cameraのプレイヤー追跡可能判定.</summary>
		private bool				isCameraEnableChasePlayer;
		/// <summary>chasePlayerCamera最大ローカルポジション(X軸).</summary>
		private int				maxChasePlayerCameraLocalPositionX;
		/// <summary>フロアマップマネージャー.</summary>
		private FloorMapManager				floorMapManager;
		/// <summary>会話.</summary>
		private GameObject			objConversation;
		/// <summary>ConversationManager.</summary>
		private ConversationManager	conversationManager;
		/// <summary>海ステージ.</summary>
		private GameObject			waterPlane;
		/// <summary>波背景.</summary>
		private GameObject			bgWave;
		/// <summary>巨大波.</summary>
		private GameObject			waveXL;

		/// <summary>Anchor transform.</summary>
		private Transform	AnchorTransform;
		/// <summary>Player transform.</summary>
		private Transform	playerTransform;
		/// <summary>chasePlayerCamera transform.</summary>
		private Transform	chasePlayerCameraTransform;
		/// <summary>フィーバー開始プレイヤーX座標位置.</summary>
		private int		feverStartPlayerPosX;
		/// <summary>プレイヤー巨大化残り時間.</summary>
		private float		playerGiganticRemainTime;
		#endregion

		#region property
		/// <summary>フィーバー中か判定.</summary>
		private	bool	isFever	= false;
		public	bool	IsFever { set{ this.isFever = value; } get{ return this.isFever; } }
		/// <summary>プレイヤー巨大化状態か判定.</summary>
		private bool	isPlayerGigantic	= false;
		public	bool	IsPlayerGigantic { set{ this.isPlayerGigantic = value; } get{ return this.isPlayerGigantic; } }
		#endregion property

		/// <summary>
		/// Awake this instance.
		/// </summary>
		private void Awake( ) {
			Anchor						= GameObject.Find ( "Anchor" );
			AnchorTransform				= Anchor.transform;
			playerController			= PlayerObj.GetComponent<PlayerController>();
			playerTransform				= PlayerObj.transform;
			chasePlayer					= ChaserCamera.GetComponent<ChasePlayer>();
			chasePlayerCameraTransform	= ChaserCamera.transform;
			uiLabelDistanceScore		= DistanceScore.GetComponent<UILabel>();
			floorMapManager				= this.GetComponent<FloorMapManager>();
			bgFever						= chasePlayerCameraTransform.FindChild( "BgFever" ).gameObject;
			objConversation				= AnchorTransform.FindChild( "Conversation" ).gameObject;
			conversationManager			= objConversation.GetComponent<ConversationManager>();
			bgWave						= chasePlayerCameraTransform.FindChild( "BgWave" ).gameObject;
			waterPlane					= chasePlayerCameraTransform.FindChild( "WaterPlane" ).gameObject;
			waveXL						= chasePlayerCameraTransform.FindChild( "WaveXL" ).gameObject;
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
		private void FixedUpdate( ) {

			if ( true  == playerController.IsDead ) {
				return;
			}

			if ( true == playerController.IsInWater ) {
				AudioManager.Instance.PlayBGM( AudioConfig.PlayerInWater );
			}

			if ( false == isPlayerGigantic ) {
				return;
			}

			if ( false == playerController.IsController ) {
				if ( true == playerController.Anim.isPlaying ) {
					playerController.SendMessage( "StopAnimation" );
				}
				return;
			}
			else {
				if ( false == playerController.Anim.isPlaying ) {
					playerController.Anim.Play();
				}
			}

			playerGiganticRemainTime -= Time.deltaTime;

			// 残り少なくなるとプレイヤー点滅.
			if ( playerGiganticRemainTime < PlayRunningGameConfig.PlayerGiganticEffectRemainTime ) {
				playerController.SetBlink( );
			}

			// 0以下で巨大化切れ.
			if ( playerGiganticRemainTime <= 0f ) {
				this.IsPlayerGigantic	= false;
				SetPlayerGiganticFinish( );
			}
		}

		/// <summary>
		/// Games the setting.
		/// </summary>
		/// <returns>The setting.</returns>
		private IEnumerator GameSetting( ) {

			while ( true ) {

				chasePlayerCameraPosition	= chasePlayerCameraTransform.position;

				if ( maxChasePlayerCameraLocalPositionX < Mathf.Floor( chasePlayerCameraPosition.x ) ) {
					maxChasePlayerCameraLocalPositionX	= (int)Mathf.Floor( chasePlayerCameraPosition.x );

					// トークが存在する場合.
					if ( ConversationConfig.IsConversation( maxChasePlayerCameraLocalPositionX ) ) {
						// 会話開始.
						StartConversation( maxChasePlayerCameraLocalPositionX );
					}

					if ( maxChasePlayerCameraLocalPositionX == PlayRunningGameConfig.SeaStageStart ) {
						bgWave.SetActive( true );
						AudioManager.Instance.PlaySE( AudioConfig.SeWave );
					}
					if ( maxChasePlayerCameraLocalPositionX == PlayRunningGameConfig.SeaStageStart + 40 ) {
						waveXL.SetActive( true );
					}

					// フィーバー状態の場合.
					if ( true == isFever ) {
						if ( feverStartPlayerPosX + PlayRunningGameConfig.FloorMapForFeverLength() <= maxChasePlayerCameraLocalPositionX ) {
							SetFeverFinish();
						}
						floorMapManager.InstantiateFloorFever( maxChasePlayerCameraLocalPositionX, (int)feverStartPlayerPosX );
					}
					else {
						floorMapManager.InstantiateFloor( maxChasePlayerCameraLocalPositionX );
					}
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

			feverStartPlayerPosX	= maxChasePlayerCameraLocalPositionX;
			Debug.Log( "feverStartPlayerPosX:" + feverStartPlayerPosX );

			// フィーバー状態に設定.
			this.IsFever	= true;
			// フィーバー背景活性化.
			bgFever.SetActive( true );
			// プレイヤーフィーバーSE.
			AudioManager.Instance.PlaySE( AudioConfig.SePlayerFever );
			// プレイヤーフィーバーエフェクト.
			playerController.SetEffect( EffectConfig.PlayerElectricEffect );
			// プレイヤーフィーバー開始エフェクト.
			playerController.SetAura( true );
			// フィーバーBGMに変更.
			AudioManager.Instance.PlayBGM( AudioConfig.PlayRunningUnrivaled );
		}

		/// <summary>
		/// フィーバー終了状態に設定.
		/// </summary>
		private void SetFeverFinish( ) {
			// フィーバー終了状態に設定.
			this.IsFever	= false;
			// フィーバー背景活性化.
			bgFever.SetActive( false );
			// プレイヤーフィーバーSE.
			AudioManager.Instance.PlaySE( AudioConfig.SePlayerFeverOut );
			// プレイヤーバリアエフェクト.
			playerController.SetAura( false );
			// プレイヤーフィーバーエフェクト.
			playerController.SetEffect( EffectConfig.PlayerElectricEffect );
			// 通常BGMに変更.
			AudioManager.Instance.PlayBGM( AudioConfig.PlayRunningDefault );
		}

		/// <summary>
		/// プレイヤー巨大化.
		/// </summary>
		private void SetPlayerGigantic( ) {

			this.IsPlayerGigantic		= true;
			// プレイヤー巨大化残り時間をセット.
			playerGiganticRemainTime	= PlayRunningGameConfig.PlayerGiganticTerm;
			// プレイヤー巨大化.
			playerController.SetGigantic( true );
		}

		/// <summary>
		/// プレイヤー巨大化終了.
		/// </summary>
		private void SetPlayerGiganticFinish( ) {
			// プレイヤー巨大化.
			playerController.SetGigantic( false );
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

				// スコアポップアップ表示.
				InstantiatePopupScore( );
			}
		}

		/// <summary>
		/// InstantiatePopupScore
		/// </summary>
		private void InstantiatePopupScore( ) {
			GameObject obj	= (GameObject)Instantiate ( Resources.Load( "Popup/PopupScore" ), Vector3.zero, Quaternion.identity );
			obj.transform.parent		= Anchor.transform;
			obj.transform.localScale	= Vector3.one;
			obj.transform.localPosition	= Vector3.zero;
		}

		/// <summary>
		/// Retire this instance.
		/// </summary>
		private void RetireGame( ) {
			Destroy ( PlayerObj );
			PlayerObj	= null;
			this.SendMessage( "OnClickButton" );
		}

		/// <summary>
		/// Raises the pause event.
		/// </summary>
		/// <param name="isPause">If set to <c>true</c> is pause.</param>
		private void OnPause( bool isPause ) {

			if ( isPause ) {
				Bg0.SendMessage( "BrakeScroll" );
				playerController.StopAnimation();
				playerController.IsController	= false;
				playerController.SpeedRight		= 0f;
				playerController.rigidbody2D.isKinematic	= true;
				chasePlayer.SpeedRight			= 0f;
			}
			else {
				Bg0.SendMessage( "StartScroll" );
				playerController.RestartAnimation();
				playerController.IsController	= true;
				playerController.SpeedRight		= PlayerConfig.DefaultSpeedRight;
				playerController.rigidbody2D.isKinematic	= false;
				chasePlayer.SpeedRight			= PlayerConfig.DefaultSpeedRight;
			}
		}

		/// <summary>
		/// 会話スタート.
		/// </summary>
		private void StartConversation( int conversationId ) {
			OnPause( true );
			objConversation.SetActive( true );
			conversationManager.ConversationId	= conversationId;

			objConversation.SendMessage( "NextTalk" );
		}

		/// <summary>
		/// 会話終了.
		/// </summary>
		private void EndConversation() {
			OnPause( false );
			Bg0.SendMessage( "StartScroll" );
			objConversation.SetActive( false );
		}

		/// Sets the active water plane.
		/// </summary>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		public void SetActiveWaterPlane( bool isActive ) {
			waterPlane.SetActive( isActive );
			bgWave.SetActive( !isActive );
		}
	}
}