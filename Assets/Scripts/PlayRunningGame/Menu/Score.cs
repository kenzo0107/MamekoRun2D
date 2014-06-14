using UnityEngine;
using System.Collections;

using PlayRunningGame;

namespace PlayRunningGame.Menu {

	public class Score : MonoBehaviour {

		#region public members.
		/// <summary>メニューobj.</summary>
		private GameObject	objMenu;
		/// <summary>矢印obj.</summary>
		private GameObject	objArrow;
		/// <summary>現在スコア</summary>
		private int currentCoinScore	= 0;
		/// <summary>フィーバー用ゲージ</summary>
		private int currentGaugeForFever	= 0;
		#endregion

		#region private members.
		private GameManager	gameManager;
		private UILabel uiLabelCoinScore;
		private UILabel uiLabelGaugeForFever;
		#endregion private members.

		/// <summary>
		/// Awake this instance.
		/// </summary>
		private void Awake( ) {
			gameManager	= this.GetComponent<GameManager>();
			objMenu	= GameObject.Find ( "Anchor/Menu" );

			Transform transformMenu = objMenu.transform;
			GameObject feverBar	= transformMenu.FindChild( "FeverBar" ).gameObject;
			Transform transformFeverBar	= feverBar.transform;
			
			uiLabelCoinScore		= transformMenu.FindChild( "CoinScore" ).GetComponent<UILabel>();
			uiLabelGaugeForFever	= transformFeverBar.FindChild( "GaugeForFever" ).GetComponent<UILabel>();
			objArrow				= transformFeverBar.FindChild( "Arrow" ).gameObject;
		}

		/// <summary>
		/// スコア加算.
		/// </summary>
		/// <param name="score">Score.</param>
		private void addScore( int score ) {

			// フィーバー状態の場合、フィーバー用ゲージは増加しない.
			if ( false == gameManager.IsFever ) {

				// フィーバー用ゲージアップ.
				currentGaugeForFever	+= score;

				if ( currentGaugeForFever >= PlayRunningGameConfig.MaxGaugeForFever ) {
					// フィーバー状態.
					gameManager.SendMessage( "SetFever" );

					currentGaugeForFever	-= PlayRunningGameConfig.MaxGaugeForFever;
				}
				// フィーバー用ゲージアップ.
				SetGuargeForFeverFormat( currentGaugeForFever );
			}
			// スコアアップ.
			currentCoinScore	+= score;
			// スコア用フォーマットにセット.
			SetCoinScoreFormat( currentCoinScore );
		}

		/// <summary>
		/// コインスコアFormatセット.
		/// </summary>
		/// <param name="score">Score.</param>
		private void SetCoinScoreFormat( int score ) {
			uiLabelCoinScore.text	= string.Format( "{0:D6}", score );
		}

		/// <summary>
		/// フィーバー用ゲージセット.
		/// </summary>
		/// <param name="score">Gauge.</param>
		private void SetGuargeForFeverFormat( int gauge ) {

			int rate	= (int)( 100 * gauge / PlayRunningGameConfig.MaxGaugeForFever );
			uiLabelGaugeForFever.text	= string.Format( "{0}%", rate );

			// メーターUp.
			objArrow.SendMessage( "MaterUp", rate );
		}
	}
}