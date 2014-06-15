using UnityEngine;
using System.Collections;

public class MoveObject : MonoBehaviour {

	public Vector3 RotateDirection	= Vector3.zero;

	public float	MoveVerticalRate;
	public float	MoveLandScapeRate;

	/// <summary>
	/// Awake this instance.
	/// </summary>
	private void FixedUpdate( ) {

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
}
