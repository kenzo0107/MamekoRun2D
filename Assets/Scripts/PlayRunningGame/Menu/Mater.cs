using UnityEngine;
using System.Collections;

public class Mater : MonoBehaviour {

	#region public members.
	/// <summary>初期値.</summary>
	public float	FirstVal	=  60f;
	/// <summary>最大値.</summary>
	public float	MaxVal		= -60f;
	/// <summary>時計周り.</summary>
	public bool		isClockwise;
	#endregion public members.

	#region private members.
	/// <summary>角度.</summary>
	private float	targetAngle;
	/// <summary>増加係数.</summary>
	private float	plusminus;
	/// <summary>現在角度.</summary>
	private float	currentAngle;

	private bool	isMove	= false;
	#endregion private members.

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void Awake( ) {
		// 時計周りの場合、「-1」.
		plusminus	= ( true == isClockwise )?	-1f	:	1f;
		currentAngle		= FirstVal;
		transform.rotation	= Quaternion.Euler( 0f, 0f, currentAngle );
	}

	/// <summary>
	/// Fixeds the update.
	/// </summary>
	private void FixedUpdate( ) {

		if ( true == isMove ) {

			if ( true == isClockwise ) {

				if ( currentAngle == targetAngle ) {
					isMove	= false;
					return;
				}
				else if ( currentAngle < targetAngle  ) {
					currentAngle	= targetAngle;
				}
				else if ( currentAngle > targetAngle ) {
					currentAngle -= 0.5f;
					transform.rotation	= Quaternion.Euler( 0f, 0f, currentAngle );
				}

			}
		}
	}

	/// <summary>
	/// Maters up.
	/// </summary>
	/// <param name="val">Value.</param>
	private void MaterUp( int percentage ) {

		if ( 0 == percentage ) return;

		isMove	= true;

		targetAngle	= plusminus * percentage * ( FirstVal - MaxVal ) / 100f + FirstVal;
	}
}
