using UnityEngine;

using System;
using System.Collections;

public class FloorMapManager : MonoBehaviour {

	#region private members.

	/// <summary>フロアマップ.</summary>
	private string floorMap			= "1111111111111111111111111111111111111111111111111111111111111111111111111111111";
	/// <summary>コイン マップ.</summary>
	private string coinMap			= "1111112343200000000011111123432000000000111111234320000000001111112343200000000";

	/// <summary>フィーバー用フロアマップ.</summary>
	private string floorMapForFever	= "11111111111111111111111111111111";
	/// <summary>フィーバー用コインマップ.</summary>
	private string coinMapForFever	= "11111123432111111111000111111111";

	/// <summary>シーソーマップ.</summary>
	private string seesawMap		= "0000001000000000000000000000000000000000000000000000000000000000000000000000000";


	/// <summary>フロアのローカルポジションY軸.</summary>
	private const float	floorLocalPosY	= -1f;

	private const int	AdevancedPlayerPosX		= 10;

	#endregion private members.

	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start( ) {
		int i;
		for ( i = 0; i <= 20; i++ ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Block", new Vector2( (float)( i - 10 ), floorLocalPosY ), Quaternion.identity );
		}
	}

	/// <summary>
	/// Instantiate the specified num.
	/// </summary>
	/// <param name="num">Number.</param>
	public void InstantiateFloor( int num ) {

		int tmpNum	= num;
		float coinLocalPosY;

		while ( tmpNum >= floorMap.Length ) {
			tmpNum	-= floorMap.Length;
		}
//		Debug.Log ( "floorMap[ tmpNum ]:" + floorMap[ tmpNum ] );
		if ( floorMap[ tmpNum ].ToString() != "0" ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Block", new Vector2( (float)( num + 10 ), floorLocalPosY ), Quaternion.identity );
		}

//		Debug.Log ( "coinMap[ tmpNum ].ToString():" + coinMap[ tmpNum ].ToString() );
		if ( coinMap[ tmpNum ].ToString() != "0" ) {
			coinLocalPosY	= Convert.ToInt32( coinMap[ tmpNum ].ToString() ) * 0.4f;
			Debug.Log ( String.Format( "coinLocalPosY:{0}", coinLocalPosY ) );
			PrefabPoolManager.Instance.instantiatePrefab( "Coin", new Vector2( (float)( num + 10 ), coinLocalPosY ), Quaternion.identity );
		}
	}
}
