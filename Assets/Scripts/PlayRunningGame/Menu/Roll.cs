using UnityEngine;
using System.Collections;

namespace PlayRunningGame.Menu {

	public class Roll: MonoBehaviour {
		
		#region public members.
		public float	RollMoveSpeed { get; set; }
		public bool		IsTranslate { get; set; }
		#endregion private members.
		
		#region private members.
		private float	defaultLocalPosionY;
		private float	localScaleY;
		#endregion private members.
		
		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake( ) {
			this.IsTranslate	= false;
			defaultLocalPosionY	= transform.localPosition.y;
			localScaleY			= transform.localScale.y;
		}
		
		/// <summary>
		/// Fixeds the update.
		/// </summary>
		void FixedUpdate( ) {
			if ( true == this.IsTranslate ) {
				transform.Translate( new Vector2( 0.0f, this.RollMoveSpeed ) );
				
				if ( Mathf.Abs( transform.localPosition.y - defaultLocalPosionY ) > localScaleY ) OnDestroy( );
			}
		}
		
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		void OnDestroy( ) {
			Destroy( this.gameObject );
		}
	}
}