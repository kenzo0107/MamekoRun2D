using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	#region public members.
	/// <summary>回転する方向.</summary>
	public Vector3 RotateDirection	= Vector3.zero;
	/// <summary>垂直方向への移動係数.</summary>
	public float	MoveVerticalRate;
	/// <summary>水平方向への移動係数.</summary>
	public float	MoveLandScapeRate;
	#endregion public members.

	#region private members.
	private Vector2	fromScaleVector;
	private Vector2 toScaleVector;
	private Vector2 fromRotateVector;
	private Vector2 toRotateVector;
	private Vector2	scaleVectorRate;
	private Vector2	rotateVectorRate;
	private	float	runTime;
	private float	timer;
	#endregion private members.

	private	bool	isStart	= false;
	public	bool	IsStart { set { isStart = value; } }

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void FixedUpdate( ) {

		if ( true == isStart ) {
			SettingMove( );
		}

		if ( Vector3.zero != RotateDirection ) {
			transform.rotation *= Quaternion.Euler( RotateDirection );
		}

		if ( 0f != MoveVerticalRate ) {
			transform.localPosition += Vector3.up * MoveVerticalRate;
		}

		if ( 0f != MoveLandScapeRate ) {
			transform.localPosition += Vector3.right * MoveLandScapeRate;
		}
	}

	/// <summary>
	/// スケール設定.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="time">所要時間.</param>
	public void SetScale( Vector2 from, Vector2 to, float time ) {
		timer		= 0f;
		fromScaleVector	= from;
		toScaleVector	= to;
		runTime		= time;
		scaleVectorRate	= ( toScaleVector - fromScaleVector ) / runTime;
	}

	/// <summary>
	/// 回転設定.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="time">所要時間.</param>
	public void SetRotation( Vector2 from, Vector2 to, float time ) {
		timer		= 0f;
		fromRotateVector	= from;
		toRotateVector		= to;
		runTime		= time;
		rotateVectorRate	= ( toRotateVector - fromRotateVector ) / runTime;
		Debug.Log ( "rotateVectorRate=" + rotateVectorRate );
	}

	/// <summary>
	/// Settings the move.
	/// </summary>
	private void SettingMove() {
		if ( timer < runTime ) {
			timer	+= Time.deltaTime;
		}
		else {
			this.IsStart	= false;
		}
			
		if ( Vector2.zero != rotateVectorRate ) {
			this.transform.rotation		= ( timer < runTime )?	Quaternion.Euler( fromRotateVector + rotateVectorRate * timer )	:	Quaternion.Euler( toRotateVector );
		}
		if( Vector2.zero !=  scaleVectorRate ) {
			this.transform.localScale	= ( timer < runTime )?	fromScaleVector + scaleVectorRate * timer	:	toScaleVector;
		}
	}
}
