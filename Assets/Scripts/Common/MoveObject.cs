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
	private Vector2	fromVector;
	private Vector2 toVector;
	private Vector2	scaleVectorRate;
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
			if ( timer < runTime ) {
				timer	+= Time.deltaTime;
				this.transform.localScale	= fromVector + scaleVectorRate * timer;
			}
			else {
				this.IsStart	= false;
				this.transform.localScale	= toVector;
			}
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
	/// Sets the scale.
	/// </summary>
	/// <param name="from">From.</param>
	/// <param name="to">To.</param>
	/// <param name="time">所要時間.</param>
	public void SetScale( Vector2 from, Vector2 to, float time ) {
		timer		= 0f;
		fromVector	= from;
		toVector	= to;
		runTime		= time;
		scaleVectorRate	= ( toVector - fromVector ) / runTime;
		Debug.Log( "scaleVectorRate=" + scaleVectorRate );
	}
}
