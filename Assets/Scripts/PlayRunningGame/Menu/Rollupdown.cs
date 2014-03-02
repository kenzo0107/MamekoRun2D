using UnityEngine;
using System.Collections;

namespace PlayRunningGame.Menu {

	public class Rollupdown : MonoBehaviour {

		#region public members.
		public GameObject HeaderRoll;
		public GameObject FooterRoll;
		#endregion

		#region private members.
		private const float RollSpeed	= 0.03f;

		private PlayRunningGame.Menu.Roll Header;
		private PlayRunningGame.Menu.Roll Footer;
		#endregion

		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake( ) {
			Header	= HeaderRoll.GetComponent<PlayRunningGame.Menu.Roll>();
			Footer	= FooterRoll.GetComponent<PlayRunningGame.Menu.Roll>();
		}

		/// <summary>
		/// Raises the enable event.
		/// </summary>
		void OnEnable( ) {
			PlayRunningGame.GameManager.StartRollUpDown += StartRollUpDown;
		}
		/// <summary>
		/// Disable this instance.
		/// </summary>
		void Disable( ) {
			PlayRunningGame.GameManager.StartRollUpDown -= StartRollUpDown;
		}

		/// <summary>
		/// ロール Up & Down開始.
		/// </summary>
		void StartRollUpDown( ) {

			Header.RollMoveSpeed	=   RollSpeed;
			Footer.RollMoveSpeed	= - RollSpeed;

			Header.IsTranslate		= true;
			Footer.IsTranslate		= true;
		}
	}
}