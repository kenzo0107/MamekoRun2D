using UnityEngine;

using System;
using System.Collections;

using PlayRunningGame;

public class FloorMapManager : MonoBehaviour {
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	private void Start( ) {
		int i;
		for ( i = 0; i <= 20; i++ ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Block", new Vector2( (float)( i - PlayRunningGameConfig.AdevancedPlayerPosX ), PlayRunningGameConfig.FloorLocalPosY ), Quaternion.identity );
		}
	}

	/// <summary>
	/// Instantiate the specified num.
	/// </summary>
	/// <param name="num">Number.</param>
	public void InstantiateFloor( int num ) {

		int tmpNum;
		string tmpFlooerMap;
		string tmpCoinMap;
		string tmpSeesawMap;
		float coinLocalPosY;

		tmpNum			= num;
		tmpFlooerMap	= PlayRunningGameConfig.FloorMap;
		tmpCoinMap		= PlayRunningGameConfig.CoinMap;
		tmpSeesawMap	= PlayRunningGameConfig.SeesawMap;

		// ループさせる.
		while ( tmpNum >= tmpFlooerMap.Length ) {
			tmpNum	-= tmpFlooerMap.Length;
		}

//		Debug.Log ( "floorMap[ tmpNum ]:" + floorMap[ tmpNum ] );
		if ( tmpFlooerMap[ tmpNum ].ToString() != "0" ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Block", new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX ), PlayRunningGameConfig.FloorLocalPosY ), Quaternion.identity );
		}

//		Debug.Log ( "coinMap[ tmpNum ].ToString():" + coinMap[ tmpNum ].ToString() );
		if ( tmpCoinMap[ tmpNum ].ToString() != "0" ) {
			coinLocalPosY	= Convert.ToInt32( tmpCoinMap[ tmpNum ].ToString() ) * PlayRunningGameConfig.CoefLocalPos;
//			Debug.Log ( String.Format( "coinLocalPosY:{0}", coinLocalPosY ) );
			PrefabPoolManager.Instance.instantiatePrefab( "Coin", new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX ), coinLocalPosY ), Quaternion.identity );
		}

		if ( tmpSeesawMap[ tmpNum ].ToString() != "0" ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Seesaw", new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX ), 0f ), Quaternion.identity );
		}
	}

	/// <summary>
	/// Instantiates the floor fever.
	/// </summary>
	/// <param name="num">Number.</param>
	public void InstantiateFloorFever( int num, int StartX ) {
		
		int tmpNum;
		string tmpFlooerMap;
		string tmpCoinMap;
		float coinLocalPosY;

		// フロアマップのどのbitを利用するか.
		tmpNum	= num - StartX;

		tmpFlooerMap	= PlayRunningGameConfig.FloorMapForFever;
		tmpCoinMap		= PlayRunningGameConfig.CoinMapForFever;

		while ( tmpNum >= tmpFlooerMap.Length ) {
			tmpNum	-= tmpFlooerMap.Length;
		}

		if ( tmpFlooerMap[ tmpNum ].ToString() != "0" ) {
			PrefabPoolManager.Instance.instantiatePrefab( "Block", new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX ), PlayRunningGameConfig.FloorLocalPosY ), Quaternion.identity );
		}

		if ( tmpCoinMap[ tmpNum ].ToString() != "0" ) {
			coinLocalPosY	= Convert.ToInt32( tmpCoinMap[ tmpNum ].ToString() ) * PlayRunningGameConfig.CoefLocalPos;
			PrefabPoolManager.Instance.instantiatePrefab( "Coin", new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX ), coinLocalPosY ), Quaternion.identity );
		}
	}
}