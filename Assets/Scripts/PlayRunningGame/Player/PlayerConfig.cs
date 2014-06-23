namespace PlayRunningGame.Player {

	/// <summary>
	/// Player config.
	/// </summary>
	public static class PlayerConfig {
		/// <summery> デフォルトジャンプ力.</summery>
		public static readonly float DefaultJumpForce	= 7f;
		/// <summery> デフォルトスピード.</summery>
		public static readonly float DefaultSpeedRight	= 0.1f;
		/// <summery>所定位置より-x方向に位置する場合のプレイヤー加速スピード.</summery>
		public static readonly float AddSpeedRight		= 0.01f;
		/// <summery>カメラ位置からのプレイヤー位置.</summery>
		public static readonly float PlayerPositionByCamera	= -3.5f;
		/// <summery>アニメーションステータス一覧.</summery>
		public enum AnimationStatusList {
			Run,
			JumpUp,
			JumpRotate,
			Sit
		}
	}
}