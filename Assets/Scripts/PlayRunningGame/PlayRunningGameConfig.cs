using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PlayRunningGame {

	public static class PlayRunningGameConfig {

		/// <summary>プレイヤーの巨大化期間.</summary>
		public static readonly float	PlayerGiganticTerm	= 8f;
		/// <summary>プレイヤーの巨大化比率.</summary>
		public static readonly float	PlayerGiganticRate	= 2f;
		/// <summary>プレイヤー点滅時の間隔.</summary>
		public static readonly float	PlayerBlinkInterval	= 0.03f;
		/// <summary>シーソー接触時のジャンプ力掛け率.</summary>
		public static readonly float	SeesawJumpForceRate	= 1.2f;
		/// <summary>プレイヤー巨大化効力切れ残り時間.</summary>
		public static readonly float	PlayerGiganticEffectRemainTime	= 1f;

		/// <summary>フィーバー用最大ゲージ.</summary>
		public static readonly int		MaxGaugeForFever	= 400;
		/// <summary>フロアのローカルポジションY軸.</summary>
		public static readonly float	FloorLocalPosY		= -1.5f;
		/// <summary>プレイヤーのX軸方向にフロアを生成する距離.</summary>
		public static readonly int		AdevancedPlayerPosX	= 10;
		/// <summary>フロアのY軸ポジションの係数.</summary>
		public static readonly float	CoefLocalPos		 = 0.5f;

		/// <summary>フロアマップ.</summary>
		public static readonly string FloorMap			= "1111111111111111111111111111111111111111111111111111111111111111111111111111111";
		public static readonly string CoinMap			= "1111100043200000000011111123432000000000111111234320000000001111112343200000000";
		public static readonly string SeesawMap			= "0000001000000000000000000000000000000000000000000000000000000000000000000000000";
		public static readonly string GiganticItemMap	= "0000005000000000000000000000000000000000000000000000000000000000000000000000000";
		public static readonly string EnemyItemMap		= "0030000000000100000000000000000000000000000000000000000000000000000000000000000";

		public enum M {
			 Block	= 1

			,Coin
			,GianticItem
			,Seasaw

			,EnemyTortoise
			,EnemyToge
		}

		public static readonly IDictionary<int, string> MapItemList = new Dictionary<int, string>( ) {
			{ (int)M.Block,			"Block" },

			{ (int)M.Coin,			"Coin" },
			{ (int)M.GianticItem,	"GianticItem" },
			{ (int)M.Seasaw,		"Seasaw" },

			{ (int)M.EnemyTortoise,	"Enemy" },
			{ (int)M.EnemyToge,		"EnemyToge" },
		};

		public static readonly int[][] StageMap	= new int[7][] {
			 new int[] {	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	0,	2,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	2,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	2,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1 }
		};



		/// <summary>Feverフロアマップ.</summary>
		public static readonly  string	FloorMapForFever	= "11111111111111111111111111111111";
		public static readonly  string	CoinMapForFever		= "11111123432111111111000111111111";

		/// <summary>
		/// Floors the length of the map for fever.
		/// </summary>
		/// <returns>The map for fever length.</returns>
		public static int FloorMapForFeverLength( ) {
			return FloorMapForFever.Length;
		}

	}
}