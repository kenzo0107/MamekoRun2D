namespace PlayRunningGame.Player {

	/// <summary>
	/// Player config.
	/// </summary>
	public static class PlayerConfig {
		// デフォルトスピード.
		public static float DefaultSpeedRight	= 0.1f;
		//  所定位置より-x方向に位置する場合のプレイヤー加速スピード.
		public static float AddSpeedRight		= 0.01f;
		// カメラ位置からのプレイヤー位置.
		public static float PlayerPositionByCamera	= -3.5f;
	}
}