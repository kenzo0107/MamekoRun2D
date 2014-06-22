using UnityEngine;
using System.Collections;

public class EnemyToge : MonoBehaviour {

	/// <summary>
	/// Start this instance.
	/// </summary>
	private IEnumerator Start( ) {
		yield return new WaitForSeconds( 0.7f );
		Transform transformObj	= this.transform;
		this.GetComponent<MoveObject>().MoveLandScapeRate	= 0f; 
		transform.rotation *= Quaternion.Euler( Vector2.zero );
		transformObj.FindChild( "togemaru" ).gameObject.SetActive( false );
		transformObj.FindChild( "togestand" ).gameObject.SetActive( true );
	}
}
