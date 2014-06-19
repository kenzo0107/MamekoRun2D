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
		/// <summary>開始位置から生成しなくてはならないフロア数.</summary>
		public static readonly int		DefaultMustInstantFloorCount	= 21;
		/// <summary>デフォルトで生成するフロア数.</summary>
		public static readonly int		DefaultInstantFloorCount	= DefaultMustInstantFloorCount - AdevancedPlayerPosX;
		/// <summary>フロアのY軸ポジションの係数.</summary>
		public static readonly float	CoefLocalPos		 = 0.5f;

		/// <summary>フロアマップアイテム.</summary>
		public enum Fl {
			 A	= 1
			,B
			,C
			,D
		}

		///<summary>Item</summary>
		public enum Item {
			 Coin	= 30
			,GiganticItem
			,Seasaw
		}

		public enum Enemy {
			 Tortoise	= 50
			,Toge
		}

		public static readonly IDictionary<int, string> MapItemList = new Dictionary<int, string>( ) {
			{ (int)Fl.A,			"Floor-a" },
			{ (int)Fl.B,			"Floor-b" },
			{ (int)Fl.C,			"Floor-c" },
			{ (int)Fl.D,			"Floor-d" },

			{ (int)Item.Coin,			"Coin" },
			{ (int)Item.GiganticItem,	"GiganticItem" },
			{ (int)Item.Seasaw,			"Seasaw" },

			{ (int)Enemy.Tortoise,	"Enemy" },
			{ (int)Enemy.Toge,		"EnemyToge" },
		};

		public static readonly int[] StartStageMap	= {
			2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3, 2, 3
		};

		/// <summary>フロアマップ.</summary>
		public static readonly int[][] StageMap	= new int[7][] {
			new int[] {	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	0,	0,	0,	31,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	30,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	51,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	50,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	0,	0,	0,	30,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	1,	2,	3,	4,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	30,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0 }
		   ,new int[] {	1,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	4,	0,	0,	0,	0,	0,	0,	0,	0,	1,	1,	2,	3,	4,	0,	1,	2,	3,	2,	3,	2,	3,	2,	4,	0,	0,	0,	0,	1,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	2,	3,	4,	0,	0,	1,	2,	3,	2,	3,	2,	3,	4,	0,	0,	0 }
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