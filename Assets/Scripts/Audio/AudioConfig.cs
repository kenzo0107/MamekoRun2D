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
	}
}