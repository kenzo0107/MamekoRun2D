using UnityEngine;
using System.Collections;

public class ChasePlayer : MonoBehaviour {

	#region public property.
	public float	SpeedRight { set; get; }
	public float	BrakeSpeed { set; get; }
	#endregion public property.

	#region constant members.
	static float BRAKE_SPEED	= 0.0005f;
	#endregion constant members.

	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake( ) {
		this.SpeedRight	= 0.0f;
		this.BrakeSpeed	= 0.0f;
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	void FixedUpdate () {
		// 右方向へのスピードが設定されている場合.
		if ( this.SpeedRight > 0.0f ) {

			// ブレーキ速度が設定されている場合、スピードを減速させる.
			if ( this.BrakeSpeed > 0.0f )	this.SpeedRight	-= this.BrakeSpeed;

			// 右方向へ移動.
			transform.Translate( new Vector2( this.SpeedRight, 0.0f ) );
		}
	}

	/// <summary>
	/// 減速させるスピード設定.
	/// </summary>
	void SetBrakeSpeed( ) {
		this.BrakeSpeed	= BRAKE_SPEED;
	}
}
