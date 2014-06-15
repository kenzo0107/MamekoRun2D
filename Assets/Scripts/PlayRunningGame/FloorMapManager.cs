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

		int LocalPosYRate;
		int tmpNum;
		tmpNum				= num;

		// ループさせる.
		while ( tmpNum >= PlayRunningGameConfig.StageMap[0].Length ) {
			tmpNum	-= PlayRunningGameConfig.StageMap[0].Length;
		}

		int i;
		for ( i = 0; i < PlayRunningGameConfig.StageMap.Length; i++ ) {
			if ( PlayRunningGameConfig.StageMap[ i ][ tmpNum ] != 0 ) {
				LocalPosYRate	= PlayRunningGameConfig.StageMap.Length - 1 - i;
				PrefabPoolManager.Instance.instantiatePrefab( PlayRunningGameConfig.MapItemList[ PlayRunningGameConfig.StageMap[ i ][ tmpNum ] ], new Vector2( (float)( num + PlayRunningGameConfig.AdevancedPlayerPosX  ), PlayRunningGameConfig.FloorLocalPosY + 0.5f * LocalPosYRate ), Quaternion.identity );
			}
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