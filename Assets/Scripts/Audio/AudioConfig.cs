using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Audio {
	
	/// <summary>
	/// Audio config.
	/// </summary>
	public static class AudioConfig {

		public static readonly IDictionary<int, string> sceneBgmList = new Dictionary<int, string>( ) {
			{ (int)Config.SceneList.Top,				"bgm0" },
			{ (int)Config.SceneList.Menu,				"bgm0" },
			{ (int)Config.SceneList.PlayRunningGame,	"bgm1" },
		};

		/// <summary>プレイランニング デフォルト状態BGM.</summary>
		public static readonly string PlayRunningDefault	= "bgm1";

		/// <summary>プレイランニング 無敵状態BGM.</summary>
		public static readonly string PlayRunningUnrivaled	= "bgm2";

		#region se.
		/// <summary>SE プレイヤージャンプ.</summary>
		public static readonly string SePlayerJump			= "se_jump";
		/// <summary>SE プレイヤージャンプ.</summary>
		public static readonly string SePlayerJumpSeesaw	= "se_seesaw";
		/// <summary>SE プレイヤーデッド.</summary>
		public static readonly string SePlayerDie			= "se_die";
		/// <summary>SE プレイヤースタミナアップ.</summary>
		public static readonly string SePlayerStaminaUp		= "se_staminaup";
		/// <summary>SE プレイヤーフィーバー.</summary>
		public static readonly string SePlayerFever			= "se_fever";
		/// <summary>SE プレイヤーフィーバー終了.</summary>
		public static readonly string SePlayerFeverOut		= "se_fever_out";

		/// <summary>SE プレイヤー巨大化.</summary>
		public static readonly string SePlayerGianticOn		= "se_doron";
		/// <summary>SE プレイヤー巨大化終了.</summary>
		public static readonly string SePlayerGianticOut	= "se_gigantic_out";
		/// <summary>SE 敵デッド.</summary>
		public static readonly string EnemyDie				= "se_enemy_die";
		#endregion se.
	}
}