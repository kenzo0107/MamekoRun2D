using UnityEngine;
using System.Collections;

public class LoadLevelManager : MonoBehaviour {

	#region public members.
	/// <summary>
	/// シーンロード直後に演出をするか判定フラグ.
	/// </summary>
	public bool IsLoadedSceneEffect = false;
	#endregion public members.

	#region private members.

	/// <summary>
	/// ロード済みか判定フラグ.
	/// </summary>
	private bool		isLoaded	= false;
	#endregion private members.

	/// <summary>
	/// Start this instance.
	/// </summary>
	IEnumerator Start( ) {
		if ( true == IsLoadedSceneEffect ) {
			yield return StartCoroutine( Zoom ( false ) );
		}
	}

	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <pam name="scene">Scene.</param>
	public IEnumerator loadLevel( int scene ) {

		if ( false == isLoaded ) {

			isLoaded	= true;

			yield return StartCoroutine( Zoom( true ) );

			Application.LoadLevel( scene );
		}
	}

	/// <summary>
	/// Zooms the out.
	/// </summary>
	/// <returns>The out.</returns>
	private IEnumerator Zoom( bool IsZoomIn ) {

		GameObject FadeInAndOut	= GetFadeInOut();

		Material sharedMaterialTexture	= FadeInAndOut.transform.FindChild( "BgFadeInOut" ).gameObject.renderer.sharedMaterial;

		// ズームイン時の初期値.
		float mainTextureScaleX		= 1f;
		float mainTextureScaleRate	= 1.1f;

		// ズームアウト時の初期値
		if ( false == IsZoomIn ) {
			mainTextureScaleX		= 500f;
			mainTextureScaleRate	= 0.9f;
		}
		// オフセット.
		float textureOffset	= -0.5f * ( mainTextureScaleX - 1 );

		// material's scale.
		sharedMaterialTexture.mainTextureScale	= new Vector2( mainTextureScaleX,	mainTextureScaleX );
		// material's offest.
		sharedMaterialTexture.mainTextureOffset	= new Vector2( textureOffset,	 	textureOffset );

		while( 0.5f < mainTextureScaleX && mainTextureScaleX <= 500.0f ) {
			
			// texture offset.
			textureOffset	= -0.5f * ( mainTextureScaleX - 1 );
			
			sharedMaterialTexture.mainTextureScale		= new Vector2( mainTextureScaleX,	mainTextureScaleX );
			sharedMaterialTexture.mainTextureOffset		= new Vector2( textureOffset,		textureOffset );
			
			// Zoom Up Rate.
			mainTextureScaleX	*= mainTextureScaleRate;
			
			yield return new WaitForSeconds( 0.0005f );
		}

		// ズームアウト時は破棄.
		if ( false == IsZoomIn ) {
			// フェードインアウトGameObject破棄.
			Destroy( FadeInAndOut );
		}

		yield return null;
	}

	/// <summary>
	/// Instantiates the fade in out.
	/// </summary>
	private static GameObject GetFadeInOut( ) {

		GameObject obj = (GameObject)Instantiate( Resources.Load( "Common/FadeInOut" ), Vector3.zero , Quaternion.identity);
		obj.transform.parent			= GameObject.Find( "Anchor" ).transform;
		obj.transform.localPosition	= new Vector3( 0, 0, - 300 );
		obj.transform.localScale		= Vector3.one;

		return obj;
	}


}
