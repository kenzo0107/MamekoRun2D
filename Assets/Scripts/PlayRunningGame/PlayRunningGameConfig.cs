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
		public static readonly int		AdevancedPlayerPosX	= 7;
		/// <summary>フロアのY軸ポジションの係数.</summary>
		public static readonly float	CoefLocalPos		 = 0.5f;

		/// <summary>フロアマップアイテム.</summary>
		public enum M {
			 Block	= 1

			,Coin
			,GiganticItem
			,Seasaw

			,EnemyTortoise
			,EnemyToge
		}

		public static readonly IDictionary<int, string> MapItemList = new Dictionary<int, string>( ) {
			{ (int)M.Block,			"Block" },

			{ (int)M.Coin,			"Coin" },
			{ (int)M.GiganticItem,	"GiganticItem" },
			{ (int)M.Seasaw,		"Seasaw" },

			{ (int)M.EnemyTortoise,	"Enemy" },
			{ (int)M.EnemyToge,		"EnemyToge" },
		};

		/// <summary>フロアマップ.</summary>
		public static readonly int[][] StageMap	= new int[7][] {
			 new int[] {	0,	0,	0,	3,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	0,	2,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	2,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	2,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	2,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	0,	0,	0,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	0,	0,	0,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1 }
		};


		/// <summary>フロアマップ Fever.</summary>
		public static readonly int[][] StageMapFever	= new int[7][] {
			 new int[] {	0,	0,	0,	0,	0,	0,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	0,	2,	2,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	0,	2,	2,	0,	2,	2,	0,	0,	0,	0,	0,	0,	0,	2,	2,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	0,	2,	2,	0,	0,	0,	2,	2,	0,	0,	0,	0,	0,	2,	2,	2,	2,	0,	0,	0,	2,	2,	0,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	0,	2,	2,	0,	0,	0,	0,	0,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	0,	0,	0,	0,	0,	0,	0 }
			,new int[] {	0,	2,	2,	0,	0,	0,	0,	0,	0,	0,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	2,	0,	0,	0,	0,	0,	0 }
			,new int[] {	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1,	1 }
		};

		/// <summary>
		/// Floors the length of the map for fever.
		/// </summary>
		/// <returns>The map for fever length.</returns>
		public static int FloorMapForFeverLength( ) {
			return StageMapFever[0].Length;
		}

	}
}