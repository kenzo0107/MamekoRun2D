﻿using UnityEngine;
using System.Collections;

using PlayRunningGame.Player;

using Audio;

namespace PlayRunningGame.Player {

	public class PlayerController : MonoBehaviour {

		#region public members.
		public float		SpeedRight { get; set; }
		public float		AddSpeedRight { get; set; }
		public float		JumpForce		= PlayerConfig.DefaultJumpForce;
		public bool			IsController	= false;
		public bool			IsDead			= false;
		public bool			IsJumpEnabaled	= false;
		public int			JumpEnableCountMax	= 1;
		#endregion

		#region private members.
		/// <summary>ゲームマネージャーobj.</summary>
		private GameObject	objGameManager;
		/// <summary>ゲームマネージャーComponent.</summary>
		private GameManager gameManager;
		/// <summary>ゲームマネージャーComponent.</summary>
		private MoveObject	moveObjectHandler;
		/// <summary>走るときの砂埃obj.</summary>
		private GameObject	dustStorm;
		/// <summary>バリアobj.</summary>
		private GameObject	barrier;
		/// <summary>バリアobjのTransform.</summary>
		private Transform	transformBarrier;
		/// <summary>デフォルトプレイヤー  LocalScale.</summary>
		private Vector2		defaultPlayerLocalScale;
		/// <summary>頭の葉っぱobj.</summary>
		private GameObject	objHeadLeaf;

		private GameObject	BoneAnimation;
		private Transform	groundCheck;				// A position marking where to check if the player is grounded.
		private bool		isGrounded		= false;
		private bool		isDoubleJump	= false;
		private int			jumpEnableCount		= 0;
		private float		jump;
		private Animation	anim;
		private Animator	animator;

		private enum AnimationStatusList {
			Run,
			JumpUp,
			JumpRotate
		}

		private AnimationStatusList	animStatus;

		/// <summary>点滅間隔.</summary>
		private float blinkIntervalTime	= PlayRunningGameConfig.PlayerBlinkInterval;
		#endregion

		/// <summary>
		/// Sets the effect.
		/// </summary>
		/// <param name="prefabName">Prefab name.</param>
		public void SetEffect( string prefabName ) {
			PrefabPoolManager.Instance.instantiatePrefab( prefabName, this.transform.localPosition, Quaternion.identity, this.transform );
		}

		/// <summary>
		/// Sets the aura.
		/// </summary>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		public void SetAura( bool isActive ) {
			if ( barrier )	barrier.SetActive( isActive );
		}

		/// <summary>
		/// Sets the blink.
		/// </summary>
		public void SetBlink( ) {
			blinkIntervalTime += Time.deltaTime;
			
			if( blinkIntervalTime >= PlayRunningGameConfig.PlayerBlinkInterval ) {
				blinkIntervalTime = 0f;
				BoneAnimation.renderer.enabled	= !BoneAnimation.renderer.enabled;
			} 
		}

		/// <summary>
		/// Sets the gigantic.
		/// </summary>
		public void SetGigantic( bool isActive ) {
			// 巨大化.
			if ( true == isActive ) {
				// プレイヤー巨大化終了SE.
				AudioManager.Instance.PlaySE( AudioConfig.SePlayerGianticOn );
				// 頭の上に葉っぱをつける.
				SetHeadLeafOnPlayer( true );
				// 巨大化エフェクト.
				SetEffect( EffectConfig.PlayerGianticEffect );

				moveObjectHandler.SetScale( defaultPlayerLocalScale, defaultPlayerLocalScale * PlayRunningGameConfig.PlayerGiganticRate, 1f );
				moveObjectHandler.IsStart	= true;
			}
			// 元のサイズに戻る.
			else {
				// プレイヤー巨大化終了SE.
				AudioManager.Instance.PlaySE( AudioConfig.SePlayerGianticOut );
				// 頭の上に葉っぱを非活性.
				SetHeadLeafOnPlayer( false );

				moveObjectHandler.SetScale( defaultPlayerLocalScale * PlayRunningGameConfig.PlayerGiganticRate, defaultPlayerLocalScale, 0.05f );
				moveObjectHandler.IsStart	= true;

				SetRenderEnabled( true );
				// プレイヤー巨大化終了エフェクト.
				SetEffect( EffectConfig.PlayerGianticOutEffect );
			}
		}

		/// <summary>
		/// Awake this instance.
		/// </summary>
		private void Awake( ) {

			// デフォルトスピード.
			SpeedRight	= PlayerConfig.DefaultSpeedRight;
			AddSpeedRight	= 0.0f;

			// Setting up references.
			objGameManager	= GameObject.Find ( "/_GameManager" );
			gameManager		= objGameManager.GetComponent<GameManager>();

			groundCheck = transform.FindChild( "groundCheck" );
			dustStorm	= transform.FindChild( "Dust Storm" ).gameObject;
			barrier		= transform.FindChild( "Barrier" ).gameObject;
			transformBarrier	= barrier.transform;

			defaultPlayerLocalScale	= transform.localScale;

			BoneAnimation	= transform.FindChild( "BoneAnimation" ).gameObject;
			anim			= BoneAnimation.GetComponent<Animation>();

			objHeadLeaf		= transform.FindChild( "BoneAnimation/Root/Total/Body/Head/Leaf" ).gameObject;

			// animation 初期値.
			animStatus	= AnimationStatusList.Run;

			moveObjectHandler	= this.GetComponent<MoveObject>();
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		private void Update () {

			// 操作可能.
			if ( true == IsController ) {
				// 地上にいるか判定.
				isGrounded = Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground") );

				if ( true == isGrounded ) {

					// アニメーションがRunステータスでない場合、Runに設定.
					if ( AnimationStatusList.Run != animStatus ) {
						animStatus	= AnimationStatusList.Run;
						SetAnimation( animStatus );
					}

					// 地上にいる場合のみ、砂埃を出す.
					if ( false == dustStorm.activeSelf ) {
						dustStorm.SetActive( true );
					}

					if ( jumpEnableCount < JumpEnableCountMax ) {
						jumpEnableCount	= JumpEnableCountMax;
					}
				}
				// 空中にいる場合は砂埃を出さない.
				else {
					dustStorm.SetActive( false );
				}

				IsJumpEnabaled	= false;
				isDoubleJump	= false;

#if UNITY_EDITOR
				if ( Input.GetMouseButtonDown(0) ) {
					if ( true == isGrounded ) {
						IsJumpEnabaled	= true;
						jump	= JumpForce;
					}
					else if( false == isGrounded && 0 < jumpEnableCount ) {
						jump	= JumpForce;
						IsJumpEnabaled	= true;
						isDoubleJump	= true;
						jumpEnableCount--;
					}
				}
#else
				if ( Input.touchCount > 0 ) {
					
					foreach ( Touch touch in Input.touches ) {
						
						// タッチ or ムーブの場合.
						if ( 
						    Input.GetTouch(0).phase == TouchPhase.Began
						    ) {

							if ( true == isGrounded ) {
								IsJumpEnabaled	= true;
							}
							else if( false == isGrounded && 0 < jumpEnableCount ) {
								IsJumpEnabaled	= true;
								isDoubleJump	= true;
								jumpEnableCount--;
							}

						}
					}
				}
#endif

				if ( true == IsJumpEnabaled && 0 < jumpEnableCount ) {
					ExecuteJump();
				}
			}
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		private void FixedUpdate ( ) {

			// プレイ不可状態.
			if ( IsDead ) {

				// プレイヤーがDead状態になったときにmp3再生.
				AudioManager.Instance.PlaySE( AudioConfig.SePlayerDie );
				//  GameManagerにプレイヤーがDead状態であることを伝える.
				objGameManager.SendMessage( "SetPlayerDie" );
				// プレイヤーGameObject破棄.
				Destroy ( this.gameObject );
			}
			// プレイ可能状態.
			else {
				transform.Translate( new Vector2( SpeedRight + AddSpeedRight, 0.0f ) );
			}
		}

		/// <summary>
		/// ジャンプ中の処理.
		/// プレイヤー上方移動.
		/// 空中ジャンプ時の演出.
		///  ジャンプ音再生.
		/// </summary>
		private void ExecuteJump( ) {
			// プレイヤージャンプ処理.
			Jump( JumpForce );
		}

		/// <summary>
		/// プレイヤージャンプ処理.
		/// </summary>
		public void Jump( float jumpforce ) {
			// アニメーション - ジャンプ.
			animStatus	= AnimationStatusList.JumpUp;
			
			// 空中ジャンプ中演出.
			if ( true == isDoubleJump ) {
				// アニメーション - 回転ジャンプ.
				animStatus	= AnimationStatusList.JumpRotate;
				
				PrefabPoolManager.Instance.instantiatePrefab( EffectConfig.PlayerJumpEffect, transform.localPosition, Quaternion.identity );
			}
			// アニメーション設定.
			SetAnimation( animStatus );
			this.rigidbody2D.velocity = Vector3.up * jumpforce;

			//  ジャンプSE再生.
			AudioManager.Instance.PlaySE( AudioConfig.SePlayerJump );
			SetAnimation( animStatus );
		}

		/// <summary>
		/// Animation設定.
		/// </summary>
		/// <param name="status">Status.</param>
		private void SetAnimation( AnimationStatusList status ) {
//			Debug.Log ( System.Convert.ToString ( status ) );
			anim.Play ( System.Convert.ToString ( status ) );
		}

		/// <summary>
		/// Raises the collision enter2 d event.
		/// </summary>
		/// <param name="coll">Coll.</param>
		private void OnCollisionEnter2D( Collision2D coll ) {

			// シーソーと衝突.
			if ( coll.gameObject.CompareTag( "Seesaw" ) ) {
				OnSeesaw( );
			}

			// 敵と衝突.
			if ( coll.gameObject.CompareTag( "KillPlayer" )  ) {
				// プレイヤー巨大化状態でない場合.
				if ( false == gameManager.IsPlayerGigantic ) {
					// プレイヤーデッド.
					IsDead	= true;
				}
				// プレイヤー巨大化状態の場合.
				else {
					Debug.Log ( "Player.OnCollisionEnter2D:KillPlayer" );
					// 敵を消滅させる.
					coll.gameObject.SendMessage( "DestroyObj" );
					return;
				}
			}

			if ( coll.gameObject.CompareTag( "EnemyHeader" ) ) {
				Debug.Log ( "Player.OnCollisionEnter2D:EnemyHeader" );
				coll.gameObject.SendMessage( "DestroyObj" );
				Jump( JumpForce );
			}
			else if ( coll.gameObject.CompareTag( "EnemyFooter" )  ) {
				Debug.Log ( "Player.OnCollisionEnter2D:EnemyFooter" );
				coll.gameObject.SendMessage( "DestroyObj" );
				Jump( -JumpForce );
			}
		}
		
		/// <summary>
		/// Raises the trigger enter2 d event.
		/// </summary>
		/// <param name="coll">Coll.</param>
		private void OnTriggerEnter2D( Collider2D coll ) {

			if ( coll.gameObject.CompareTag( "EnemyHeader" )  ) {
				Debug.Log ( "OnTriggerEnter2D:EnemyHeader" );
				coll.gameObject.SendMessage( "DestroyObj" );
				Jump( JumpForce );
			}
			if ( coll.gameObject.CompareTag( "EnemyFooter" )  ) {
				Debug.Log ( "OnTriggerEnter2D:EnemyFooter" );
				coll.gameObject.SendMessage( "DestroyObj" );
				Jump( -JumpForce );
			}

			if ( false == gameManager.IsPlayerGigantic ) {
				// 敵と衝突.
				if ( coll.gameObject.CompareTag( "KillPlayer" )  ) {
					IsDead	= true;
				}
			}
		}

		/// <summary>
		/// Speeds up.
		/// </summary>
		private void SpeedUp( ) {
			AddSpeedRight	= PlayerConfig.AddSpeedRight;
		}

		/// <summary>
		/// Sets the current speed.
		/// </summary>
		private void SetCurrentSpeed( ) {
			AddSpeedRight	= 0f;
		}

		/// <summary>
		/// Sets the is controller.
		/// </summary>
		private void setIsController( bool boolean ) {
			IsController	= boolean;
		}

		/// <summary>
		/// Raises the seesaw event.
		/// </summary>
		private void OnSeesaw( ) {
			Jump( JumpForce * PlayRunningGameConfig.SeesawJumpForceRate );
		}
		
		/// <summary>
		/// 頭の葉っぱActive化.
		/// </summary>
		/// <param name="isActive">If set to <c>true</c> is active.</param>
		private void SetHeadLeafOnPlayer( bool isActive ) {
			objHeadLeaf.SetActive( isActive );
		}

		/// <summary>
		/// Sets the render enabled.
		/// </summary>
		/// <param name="isEnabled">If set to <c>true</c> is enabled.</param>
		private void SetRenderEnabled( bool isEnabled ) {
			BoneAnimation.renderer.enabled	= isEnabled;
		}
	}
}