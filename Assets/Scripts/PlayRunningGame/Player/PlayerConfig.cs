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

		/// <summary>ジャンプ期間.</summary>
		public static readonly float DefaultJumpTerm	= 0.3f;
		/// <summary>接地判定までの距離.</summary>
		public static readonly float DefaultGroundCheck		= 0.515f;
		public static readonly float GiganticGroundCheck	= 1.03f;

		/// <summery>アニメーションステータス一覧.</summery>
		public enum AnimationStatusList {
			Run,
			JumpUp,
			JumpRotate,
			Sit
		}
	}
}