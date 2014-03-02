using UnityEngine;
using System.Collections;

namespace PlayRunningGame.Menu {

	public class Score : MonoBehaviour {

		#region public members.
		public UILabel CoinScore;
		#endregion

		public int CurrentScore { get; set; }

		/// <summary>
		/// Adds the score.
		/// </summary>
		/// <param name="score">Score.</param>
		void addScore( int score ) {
			this.CurrentScore	+= score;
			setScore( );
		}

		/// <summary>
		/// Sets the score.
		/// </summary>
		void setScore( ) {
			CoinScore.text	= string.Format( "{0:D6}", this.CurrentScore );
		}
	}
}