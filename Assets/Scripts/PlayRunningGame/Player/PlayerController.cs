using UnityEngine;
using System.Collections;

namespace PlayRunningGame.Player {

	public class PlayerController : MonoBehaviour {

		#region public members.
		public float		SpeedRight { get; set; }
		public float		AddSpeedRight { get; set; }
		public float		JumpForce;
		public bool			IsController	= false;
		public bool			IsDead			= false;
		public bool			IsJumpEnabaled	= false;
		public int			JumpEnableCountMax	= 1;
		#endregion

		#region private members.
		private GameObject	gameManager;
		private GameObject	dustStorm;
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

		#endregion

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake( ) {

			// デフォルトスピード.
			SpeedRight	= PlayRunningGame.Player.PlayerConfig.DefaultSpeedRight;
			AddSpeedRight	= 0.0f;

			// Setting up references.
			gameManager		= GameObject.Find ( "_GameManager" );

			groundCheck = transform.FindChild( "groundCheck" );
			dustStorm	= transform.FindChild( "Dust Storm" ).gameObject;

			GameObject BoneAnimation	= transform.FindChild( "BoneAnimation" ).gameObject;
			anim		= BoneAnimation.GetComponent<Animation>();

			// animation 初期値.
			animStatus	= AnimationStatusList.Run;
		}

		/// <summary>
		/// Update this instance.
		/// </summary>
		void Update () {

			// 操作可能.
			if ( true == IsController ) {
				// 地上にいるか判定.
				isGrounded = Physics2D.Linecast( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground") );

				if ( true == isGrounded ) {

					// アニメーションがRunステータスでない場合、Runに設定.
					if ( AnimationStatusList.Run != animStatus ) {
						animStatus	= AnimationStatusList.Run;
						setAnimation( animStatus );
					}

					// 地上にいる場合のみ、砂埃を出す.
					if ( false == dustStorm.activeSelf ) {
						dustStorm.SetActive( true );
					}

					if ( jumpEnableCount < JumpEnableCountMax ) {
						jumpEnableCount	= JumpEnableCountMax;
					}
				}
				// // 空中にいる場合は砂埃を出さない.
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
					executeJump();
				}
			}
		}

		/// <summary>
		/// Fixeds the update.
		/// </summary>
		void FixedUpdate ( ) {

			// プレイ不可状態.
			if ( IsDead ) {

				// プレイヤーがDead状態になったときにmp3再生.
				Audio.AudioManager.Instance.PlaySE( "se_die" );
				//  GameManagerにプレイヤーがDead状態であることを伝える.
				gameManager.SendMessage( "setPlayerDie" );
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
		void executeJump( ) {

			// animation status Jump.
			animStatus	= AnimationStatusList.JumpUp;

			// 空中ジャンプ中演出.
			if ( true == isDoubleJump ) {
				// 回転ジャンプ.
				animStatus	= AnimationStatusList.JumpRotate;

				PrefabPoolManager.Instance.instantiatePrefab( "CFXM3_Hit_Light_B_Air", transform.localPosition, Quaternion.identity );
//				GameObject jumpEffect	= (GameObject)Instantiate( Resources.Load ( "JMO Assets/Cartoon FX/CFX3 Prefabs (Mobile)/Light/CFXM3_Hit_Light_B_Air" ), Vector3.zero, Quaternion.identity );
			}

			setAnimation( animStatus );

			// プレイヤージャンプ処理.
			Jump( JumpForce );
			//  ジャンプSE再生.
			Audio.AudioManager.Instance.PlaySE( "se_jump" );
		}

		/// <summary>
		/// プレイヤージャンプ処理.
		/// </summary>
		void Jump( float jumpforce ) {
			this.rigidbody2D.velocity = Vector3.up * jumpforce;
	//		rigidbody2D.AddForce( new Vector2( 0f, jumpforce ) );
		}

		/// <summary>
		/// Sets the animation.
		/// </summary>
		/// <param name="status">Status.</param>
		private void setAnimation( AnimationStatusList status ) {
			Debug.Log ( System.Convert.ToString ( status ) );
			anim.Play ( System.Convert.ToString ( status ) );
		}

		/// <summary>
		/// Raises the collision enter2 d event.
		/// </summary>
		/// <param name="coll">Coll.</param>
		void OnCollisionEnter2D( Collision2D coll ) {

	//		Debug.Log ( "coll.gameObject.name=" + coll.gameObject.name );

			if ( coll.gameObject.CompareTag( "KillPlayer" )  ) {
				Debug.Log ( "OnCollisionEnter2D::KillPlayer" );
				IsDead	= true;
			}
		}

		/// <summary>
		/// Raises the trigger enter2 d event.
		/// </summary>
		/// <param name="coll">Coll.</param>
		void OnTriggerEnter2D( Collider2D coll ) {

	//		Debug.Log ( "coll.gameObject.name=" + coll.gameObject.name );

			if ( coll.gameObject.CompareTag( "KillPlayer" )  ) {
//				Debug.Log ( "OnTriggerEnter2D::KillPlayer" );
				IsDead	= true;
			}
		}

		/// <summary>
		/// Speeds up.
		/// </summary>
		void SpeedUp( ) {
			AddSpeedRight	= PlayRunningGame.Player.PlayerConfig.AddSpeedRight;
		}

		/// <summary>
		/// Sets the current speed.
		/// </summary>
		void setCurrentSpeed( ) {
			AddSpeedRight	= 0f;
		}

		/// <summary>
		/// Sets the is controller.
		/// </summary>
		void setIsController( bool boolean ) {
			IsController	= boolean;
		}
	}
}