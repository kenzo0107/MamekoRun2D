using UnityEngine;
using System.Collections;

namespace PlayRunningGame {

	public static class PlayRunningGameConfig {

		/// <summary>フィーバー用最大ゲージ.</summary>
		public static readonly int		MaxGaugeForFever	= 100;
		/// <summary>フロアのローカルポジションY軸.</summary>
		public static readonly float	FloorLocalPosY		= -1f;
		/// <summary>プレイヤーのX軸方向にフロアを生成する距離.</summary>
		public static readonly int		AdevancedPlayerPosX	= 10;
		/// <summary>フロアのY軸ポジションの係数.</summary>
		public static readonly float	CoefLocalPos		 = 0.5f;

		/// <summary>フロアマップ.</summary>
		public static readonly string FloorMap		= "1111111111111111111111111111111111111111111111111111111111111111111111111111111";
		public static readonly string CoinMap		= "1111112343200000000011111123432000000000111111234320000000001111112343200000000";
		public static readonly string SeesawMap		= "0000001000000000000000000000000000000000000000000000000000000000000000000000000";

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