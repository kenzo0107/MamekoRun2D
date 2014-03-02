using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class DestroyCollisionGameObject : MonoBehaviour {

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnTriggerEnter2D( Collider2D coll ) {

		if ( coll.gameObject.name.Contains( "DeadLineY" ) ) {

			// 破棄するFloorの番号.
			int FloorNumber	= (int)System.Convert.ToDouble( Regex.Replace( this.gameObject.transform.parent.gameObject.name, @"[^\d]", string.Empty) );

			// 新たに生成するFloor.
			SetActiveFloor( FloorNumber + 3 );

			Destroy( this.gameObject.transform.parent.gameObject );
		}
	}

	/// <summary>
	/// Sets the active floor.
	/// </summary>
	/// <param name="instantiateFloorNumber">Instantiate floor number.</param>
	private void SetActiveFloor( int instantiateFloorNumber ) {
		GameObject Floor	= this.gameObject.transform.parent.transform.parent.FindChild( string.Format( "Floor{0}", instantiateFloorNumber ) ).gameObject;
//		GameObject Floor	= GameObject.Find( string.Format( "Floor/Floor{0}", instantiateFloorNumber ) );
		Floor.transform.localPosition	= new Vector2( 50 * ( instantiateFloorNumber - 1 ), 0 );
		Floor.SetActive( true );
	}
}